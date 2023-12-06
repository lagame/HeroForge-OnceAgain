using HeroForge_OnceAgain.Models;
using HeroForge_OnceAgain.Utils;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;

namespace HeroForge_OnceAgain
{
    public partial class FrmLogin : Form
    {

        private static FrmLogin _defaultInstance;
        public static string approvalCode;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // client configuration

        public const string clientID = "122225228347-bnv4dakc7icmevkg33806vun9hcl4c6c.apps.googleusercontent.com";
        
        //"662959790489-sqt6ja2lqj5hnd4o7g7ki4jg7j0okvl5.apps.googleusercontent.com";
        public const string clientSecret = "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";
        //"pJFA5xdGQ-MbRdEniXIseRH-";
        public const string redirectURI = "http://localhost:8080";
        public bool flag = true;

        const string authorizationEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
        const string tokenEndpoint = "https://www.googleapis.com/oauth2/v4/token";
        const string userInfoEndpoint = "https://www.googleapis.com/oauth2/v3/userinfo";

        public static FrmLogin Default
        {
            get
            {
                if (_defaultInstance == null || _defaultInstance.IsDisposed)
                {
                    _defaultInstance = new FrmLogin();

                }
                return _defaultInstance;
            }
            set => _defaultInstance = value;
        }
        public FrmLogin()
        {
            //InitializeComponent();
            Reload();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            guna2AnimateWindow1.SetAnimateWindow(this);    
            guna2ShadowForm1.SetShadowForm(this);
            //Localization();
        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private async void btn_LoginGoogle_Click(object sender, EventArgs e)
        {
            // Generates state and PKCE values.
            string state = randomDataBase64url(32);
            string code_verifier = randomDataBase64url(32);
            string code_challenge = base64urlencodeNoPadding(sha256(code_verifier));
            const string code_challenge_method = "S256";

            // Creates a redirect URI using an available port on the loopback address.
            string redirectURI = string.Format("http://{0}:{1}/", IPAddress.Loopback, GetRandomUnusedPort());
            output("redirect URI: " + redirectURI);

            // Creates an HttpListener to listen for requests on that redirect URI.
            var http = new HttpListener();
            http.Prefixes.Add(redirectURI);
            output("Listening..");
            http.Start();

            // Creates the OAuth 2.0 authorization request.
            string authorizationRequest = string.Format("{0}?response_type=code&scope=email%20profile&openid%20profile&redirect_uri={1}&client_id={2}&state={3}&code_challenge={4}&code_challenge_method={5}",
                authorizationEndpoint,
                System.Uri.EscapeDataString(redirectURI),
                clientID,
                state,
                code_challenge,
                code_challenge_method);

            // Opens request in the browser.
            System.Diagnostics.Process.Start(authorizationRequest);

            // Waits for the OAuth authorization response.
            var context = await http.GetContextAsync();

            // Brings this app back to the foreground.
            this.Activate();

            // Sends an HTTP response to the browser.
            var response = context.Response;
            string responseString = string.Format("<html><head><meta http-equiv='refresh' content='10;url=https://google.com'></head><body>Please return to the app.</body></html>");
            var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            var responseOutput = response.OutputStream;
            Task responseTask = responseOutput.WriteAsync(buffer, 0, buffer.Length).ContinueWith((task) =>
            {
                responseOutput.Close();
                http.Stop();
                Console.WriteLine("HTTP server stopped.");
            });

            // Checks for errors.
            if (context.Request.QueryString.Get("error") != null)
            {
                output(String.Format("OAuth authorization error: {0}.", context.Request.QueryString.Get("error")));
                return;
            }
            if (context.Request.QueryString.Get("code") == null
                || context.Request.QueryString.Get("state") == null)
            {
                output("Malformed authorization response. " + context.Request.QueryString);
                return;
            }

            // extracts the code
            var code = context.Request.QueryString.Get("code");
            var incoming_state = context.Request.QueryString.Get("state");

            // Compares the receieved state to the expected value, to ensure that
            // this app made the request which resulted in authorization.
            if (incoming_state != state)
            {
                output(String.Format("Received request with invalid state ({0})", incoming_state));
                return;
            }
            output("Authorization code: " + code);

            // Starts the code exchange at the Token Endpoint.
            performCodeExchange(code, code_verifier, redirectURI);
        }
        /// <summary>
        /// Returns the SHA256 hash of the input string.
        /// </summary>
        /// <param name="inputStirng"></param>
        /// <returns></returns>
        public static byte[] sha256(string inputStirng)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(inputStirng);
            SHA256Managed sha256 = new SHA256Managed();
            return sha256.ComputeHash(bytes);
        }
        /// <summary>
        /// Base64url no-padding encodes the given input buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string base64urlencodeNoPadding(byte[] buffer)
        {
            string base64 = Convert.ToBase64String(buffer);

            // Converts base64 to base64url.
            base64 = base64.Replace("+", "-");
            base64 = base64.Replace("/", "_");
            // Strips padding.
            base64 = base64.Replace("=", "");

            return base64;
        }
        /// <summary>
        /// Returns URI-safe data with a given input length.
        /// </summary>
        /// <param name="length">Input length (nb. output will be longer)</param>
        /// <returns></returns>
        public static string randomDataBase64url(uint length)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[length];
            rng.GetBytes(bytes);
            return base64urlencodeNoPadding(bytes);
        }
        async void performCodeExchange(string code, string code_verifier, string redirectURI)
        {
            output("Exchanging code for tokens...");

            // builds the  request
            string tokenRequestURI = "https://www.googleapis.com/oauth2/v4/token";
            string tokenRequestBody = string.Format("code={0}&redirect_uri={1}&client_id={2}&code_verifier={3}&client_secret={4}&email%20profile&grant_type=authorization_code",
                code,
                System.Uri.EscapeDataString(redirectURI),
                clientID,
                code_verifier,
                clientSecret
                );

            // sends the request
            HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create(tokenRequestURI);
            tokenRequest.Method = "POST";
            tokenRequest.ContentType = "application/x-www-form-urlencoded";
            tokenRequest.Accept = "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            byte[] _byteVersion = Encoding.ASCII.GetBytes(tokenRequestBody);
            tokenRequest.ContentLength = _byteVersion.Length;
            Stream stream = tokenRequest.GetRequestStream();
            await stream.WriteAsync(_byteVersion, 0, _byteVersion.Length);
            stream.Close();

            try
            {
                // gets the response
                WebResponse tokenResponse = await tokenRequest.GetResponseAsync();
                using (StreamReader reader = new StreamReader(tokenResponse.GetResponseStream()))
                {
                    // reads response body
                    string responseText = await reader.ReadToEndAsync();
                    output(responseText);

                    // converts to dictionary
                    Dictionary<string, string> tokenEndpointDecoded = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);

                    string access_token = tokenEndpointDecoded["access_token"];
                    userinfoCall(access_token);
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    var response = ex.Response as HttpWebResponse;
                    if (response != null)
                    {
                        output("HTTP: " + response.StatusCode);
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            // reads response body
                            string responseText = await reader.ReadToEndAsync();
                            output(responseText);
                        }
                    }

                }
            }
        }

        void verifyUser(string userEmail, string userName)
        {
            //string connectionString = "Server=heroforge.mysql.dbaas.com.br;Database=heroforge;User Id=heroforge;Password=Xm@753951;";
            string connectionString = AppGlobal.ConnectionString;
            //string connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    // Abre a conexão com o banco de dados
                    connection.Open();

                    // Verifique se o usuário já existe no banco de dados
                    string query = "SELECT * FROM Users WHERE UserEmail = @UserEmail";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserEmail", userEmail);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Usuário encontrado no banco de dados
                                int userID = reader.GetInt32("UserID");
                                string username = reader.GetString("Username");

                                // Crie um DTO do usuário
                                UserDTO userDTO = new UserDTO(userID, username, userEmail);

                                // Faça o que precisar com o usuário encontrado
                                Console.WriteLine($"Usuário encontrado: {userDTO.Username} ({userDTO.UserEmail})");
                                reader.Close();
                                Form frm1 = new Form1();
                                frm1.Show();
                                this.Visible = false;
                            }
                            else
                            {
                                reader.Close();
                                // Usuário não encontrado no banco de dados
                                Console.WriteLine("Usuário não encontrado, vamos cadastrar...");

                                // Usuário não encontrado no banco de dados, então vamos cadastrá-lo
                                string insertQuery = "INSERT INTO Users (Username, UserEmail) VALUES (@Username, @UserEmail)";
                                using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection))
                                {
                                    // Defina os parâmetros para a inserção
                                    insertCmd.Parameters.AddWithValue("@Username", userName); // Defina o nome conforme necessário
                                    insertCmd.Parameters.AddWithValue("@UserEmail", userEmail);

                                    // Execute a inserção
                                    int rowsAffected = insertCmd.ExecuteNonQuery();

                                    if (rowsAffected > 0)
                                    {
                                        Console.WriteLine("Novo usuário cadastrado com sucesso!");
                                        Form frm1 = new Form1();
                                        frm1.Show();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Erro ao cadastrar novo usuário.");
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao conectar ao banco de dados: {ex.Message}");
                }
            }
        }

        public class UserDTO
        {
            public int UserID { get; set; }
            public string Username { get; set; }
            public string UserEmail { get; set; }
            // Adicione outras propriedades conforme necessário

            public UserDTO(int userID, string username, string userEmail)
            {
                UserID = userID;
                Username = username;
                UserEmail = userEmail;
                // Inicialize outras propriedades conforme necessário
            }
        }


        async void userinfoCall(string access_token)
        {
            output("Making API Call to Userinfo...");

            // builds the  request
            string userinfoRequestURI = "https://www.googleapis.com/oauth2/v3/userinfo";

            // sends the request
            HttpWebRequest userinfoRequest = (HttpWebRequest)WebRequest.Create(userinfoRequestURI);
            userinfoRequest.Method = "GET";
            userinfoRequest.Headers.Add(string.Format("Authorization: Bearer {0}", access_token));
            userinfoRequest.ContentType = "application/x-www-form-urlencoded";
            userinfoRequest.Accept = "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

            // gets the response
            WebResponse userinfoResponse = await userinfoRequest.GetResponseAsync();
            using (StreamReader userinfoResponseReader = new StreamReader(userinfoResponse.GetResponseStream()))
            {
                // reads response body
                string userinfoResponseText = await userinfoResponseReader.ReadToEndAsync();
                output(userinfoResponseText);

                // Parse dos dados do usuário
                var userData = JsonConvert.DeserializeObject<Dictionary<string, string>>(userinfoResponseText);
                var userName = userData["name"];
                var userEmail = userData["email"];

                // Chame o método para verificar ou cadastrar o usuário no banco de dados
                //VerifyOrRegisterUser(userName, userEmail);
                verifyUser(userEmail, userName);
            }
        }


        /// <summary>
        /// Appends the given string to the on-screen log, and the debug console.
        /// </summary>
        /// <param name="output">string to be appended</param>
        public void output(string output)
        {
            textBoxOutput.Text = textBoxOutput.Text + output + Environment.NewLine;
            log.Info(output);
            //Console.WriteLine(output);
        }

        public static int GetRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }

        private string GetProfileInfo(string accessToken)
        {
            var url = $"https://www.googleapis.com/oauth2/v3/userinfo?access_token={accessToken}";
            using (var wc = new WebClient())
            {
                wc.Headers.Add(HttpRequestHeader.AcceptCharset, "utf-8");
                wc.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; …) Gecko/20100101 Firefox/55.0");
                wc.Encoding = Encoding.UTF8;
                return wc.DownloadString(url);
            }
        }
        public bool GetAppoveCodeGoogle()
        {
            Process[] procsChrome = Process.GetProcessesByName("chrome");
            foreach (Process chrome in procsChrome)
            {
                if (chrome.MainWindowHandle == IntPtr.Zero)
                    continue;

                AutomationElement element = AutomationElement.FromHandle(chrome.MainWindowHandle);
                if (element != null)
                {
                    Condition conditions = new AndCondition(
                   new PropertyCondition(AutomationElement.ProcessIdProperty, chrome.Id),
                   new PropertyCondition(AutomationElement.IsControlElementProperty, true),
                   new PropertyCondition(AutomationElement.IsContentElementProperty, true),
                   new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));

                    AutomationElement elementx = element.FindFirst(TreeScope.Descendants, conditions);
                    if (elementx != null)
                    {
                        var url = ((ValuePattern)elementx.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
                        if (url.Contains("flowName=GeneralOAuthFlow"))
                        {
                            var arr = url.Split('&');
                            approvalCode = WebUtility.HtmlDecode(arr[arr.Length - 1].Replace("approvalCode=", ""));
                            return false;
                        }
                    }                   

                }


            }
            return true;
        }

        public void Localization()
        {
            switch (Properties.Settings.Default.LanguageIndex)
            {
                case 0:
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
                    break;
                case 1:
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pt-BR");
                    break;
            }

        }

        public void Reload()
        {
            switch (Properties.Settings.Default.LanguageIndex)
            {
                case 0:
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
                    break;
                case 1:
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pt-BR");
                    break;
            }
            this.Controls.Clear();
            InitializeComponent();            
        }
    }
}
