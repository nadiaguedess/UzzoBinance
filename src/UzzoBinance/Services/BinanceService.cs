using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UzzoBinance.Models.DataBase;
using UzzoBinance.Models.UzzoBinance;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace UzzoBinance.Services
{
    public class BinanceService
    {
        private readonly UzzoBinanceContext db_UzzoBinance;
        private IConfiguration _configuration;
        public BinanceService(IConfiguration Configuration, UzzoBinanceContext db_UzzoBinance)
        {
            this.db_UzzoBinance = db_UzzoBinance;
            _configuration = Configuration;
        }

        public async Task<string> SearchPrice()
        {
            try
            {
                SearchPrice obj_SearchPrice = new SearchPrice();

                //Utilizando HttClient para a chamada da requisição
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Realizando o post para criação de uma nova ordem na Binance
                    HttpResponseMessage httpResponse = httpClient.GetAsync($"{_configuration.GetSection("Binance:SymbolPrice").Value}{"BTCUSDT"}").Result;
                    var varReceiveStream = await httpResponse.Content.ReadAsStringAsync();

                    //Serealizando o retorno em um objeto do tipo Model SeachPrice
                    dynamic objReturn = Newtonsoft.Json.JsonConvert.DeserializeObject<SearchPrice>(varReceiveStream);

                    if (objReturn != null)
                    {
                        //Inserindo Preço de Compra e Preço de Venda da Criptomoeda no banco de dados
                        var var_InsertSymbolPrice = new SymbolPrice
                        {
                            symbol = objReturn.symbol,
                            bidPrice = Convert.ToDecimal(objReturn.bidPrice), //Compra
                            askPrice = Convert.ToDecimal(objReturn.askPrice), //Venda
                            dateAndTimePrice = DateTime.Now
                        };
                        
                        db_UzzoBinance.Add(var_InsertSymbolPrice);
                        db_UzzoBinance.SaveChanges();
                        //Retirar o Tracked da tabela para que seja possivel instanciar a mesma novamente
                        db_UzzoBinance.Entry(var_InsertSymbolPrice).State = EntityState.Detached;

                    }

                }

                return obj_SearchPrice.lastId.ToString();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }            
        }

        public async Task<string> NewOrder(NewOrder obj_NewOrder)
        {
            try
            {
                //Gerando número aleatório para o id da ordem
                Random randnewClientOrderId = new Random();
                MD5 shaMD5Hash = MD5.Create();

                //Criando o id da ordem com base em um número aleatório convertido para hash
                obj_NewOrder.newClientOrderId = GetHashOrderId(shaMD5Hash, randnewClientOrderId.Next().ToString());

                //Convertendo o body da requisição em json do Model NewOrder
                var jsonNewOrder = JsonConvert.SerializeObject(obj_NewOrder);

                //Utilizando HttClient para a chamada da requisição
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //Adicionando autorização com base na key da Binace
                    httpClient.DefaultRequestHeaders.Add("X-MBX-APIKEY", _configuration.GetSection("Binance:API_Key").Value);
                    obj_NewOrder.timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                    var sha256Signature = new Dictionary<string, string>()
                {
                   { nameof(obj_NewOrder.symbol).ToString(), obj_NewOrder.symbol },
                   { nameof(obj_NewOrder.side).ToString(), obj_NewOrder.side },
                   { nameof(obj_NewOrder.type).ToString(), obj_NewOrder.type },
                   { nameof(obj_NewOrder.timeInForce).ToString(), obj_NewOrder.timeInForce },
                   { nameof(obj_NewOrder.quantity).ToString(), obj_NewOrder.quantity.ToString().Replace(",",".") },
                   { nameof(obj_NewOrder.price).ToString(), obj_NewOrder.price.ToString() },
                   { nameof(obj_NewOrder.newClientOrderId).ToString(), obj_NewOrder.newClientOrderId },
                   { nameof(obj_NewOrder.recvWindow).ToString(), obj_NewOrder.recvWindow.ToString() },
                   { nameof(obj_NewOrder.timestamp).ToString(), obj_NewOrder.timestamp.ToString() }

                };

                    var listsha256Signature = new List<string>();

                    foreach (var item in sha256Signature)
                    {
                        listsha256Signature.Add(item.Key + "=" + item.Value);
                    }

                    obj_NewOrder.signature = GetHashSignature(string.Join("&", listsha256Signature));

                    var parameters = new Dictionary<string, string>()
                {
                   { nameof(obj_NewOrder.symbol).ToString(), obj_NewOrder.symbol },
                   { nameof(obj_NewOrder.side).ToString(), obj_NewOrder.side },
                   { nameof(obj_NewOrder.type).ToString(), obj_NewOrder.type },
                   { nameof(obj_NewOrder.timeInForce).ToString(), obj_NewOrder.timeInForce },
                   { nameof(obj_NewOrder.quantity).ToString(), obj_NewOrder.quantity.ToString().Replace(",",".") },
                   { nameof(obj_NewOrder.price).ToString(), obj_NewOrder.price.ToString() },
                   { nameof(obj_NewOrder.newClientOrderId).ToString(), obj_NewOrder.newClientOrderId },
                   { nameof(obj_NewOrder.recvWindow).ToString(), obj_NewOrder.recvWindow.ToString() },
                   { nameof(obj_NewOrder.timestamp).ToString(), obj_NewOrder.timestamp.ToString() },
                   { nameof(obj_NewOrder.signature).ToString(), obj_NewOrder.signature }
                };
                    var encodedContent = new FormUrlEncodedContent(parameters);

                    //Realizando o post para criação de uma nova ordem na Binance
                    HttpResponseMessage response = httpClient.PostAsync(_configuration.GetSection("Binance:NewOrder").Value, encodedContent).Result;
                    var receiveStream = await response.Content.ReadAsStringAsync();

                    dynamic objResposta = Newtonsoft.Json.JsonConvert.DeserializeObject(receiveStream);
                    if (objResposta.id != null)
                    {
                        return objResposta.id;
                    }
                    else
                    {
                        return objResposta.ToString().Replace(":", "").Replace("{", "").Replace("}", "").Replace("\"", "");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
            
        }

        private string GetHashOrderId(HashAlgorithm hashAlgorithm, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        //Converte os parametros da requisição para HMACSHA256, gerando a assinatura para a Binance
        public String GetHashSignature(String text)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();

            Byte[] textBytes = encoding.GetBytes(text);
            Byte[] keyBytes = encoding.GetBytes(_configuration.GetSection("Binance:Secret_Key").Value);

            Byte[] hashBytes;

            using (HMACSHA256 hash = new HMACSHA256(keyBytes))
                hashBytes = hash.ComputeHash(textBytes);

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
