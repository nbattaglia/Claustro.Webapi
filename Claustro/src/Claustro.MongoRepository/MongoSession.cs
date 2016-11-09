using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Claustro.MongoRepository
{
    public interface IMongoSession

    {
        IMongoDatabase Db { get; }
        IMongoClient Session { get; }

    }
    public class MongoSession : IMongoSession
    {


        private IMongoDatabase _db;
        private IMongoClient _client;
        public MongoSession(ICreds creds)
        {

            byte[] data = Convert.FromBase64String(creds.cert);

            var clientSettings = MongoClientSettings.FromUrl(new MongoUrl(creds.uri));
            clientSettings.SslSettings = new SslSettings();
            clientSettings.UseSsl = true;
            clientSettings.SslSettings.ClientCertificates = new List<X509Certificate>()
    {
        new X509Certificate(data)
    };
            clientSettings.SslSettings.EnabledSslProtocols = SslProtocols.Tls;
            clientSettings.SslSettings.ClientCertificateSelectionCallback =
                (sender, host, certificates, certificate, issuers) => clientSettings.SslSettings.ClientCertificates.ToList()[0];
            clientSettings.SslSettings.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
            _client = new MongoClient(clientSettings);


                        //_client = new MongoClient(creds.uri);
            _db = _client.GetDatabase("claustro");
        }




        IMongoClient IMongoSession.Session
        {
            get
            {
                return _client;
            }
        }

        IMongoDatabase IMongoSession.Db
        {
            get
            {
                return _db;
            }
        }
    }
}
