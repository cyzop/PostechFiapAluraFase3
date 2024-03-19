

using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using PosTech.CadPac.Domain.Entities;
using PosTech.CadPac.Domain.Repositories;
using PosTech.CadPac.Domain.Shared.Converter;
using PosTech.CadPac.Repository.DataModel;
using PosTech.CadPac.Repository.Extensions;

namespace PosTech.CadPac.Repository
{
    public class PacienteRepository : IPacienteRepository
    {
        private readonly IMongoCollection<PacienteDataModel> _database;
        private readonly IConverter<Paciente, PacienteDataModel> _pacienteConverter;
        private readonly IConverter<PacienteDataModel, Paciente> _pacienteModelConverter;
        private readonly IConfiguration _configuration;

        public PacienteRepository(RepositorySettings config, IConverter<Paciente, PacienteDataModel> pacienteConverter, IConverter<PacienteDataModel, Paciente> pacienteModelConverter, IConfiguration configuration)
        {
            //string connectionString = string.Format(config.ConnectionString, config.Secret);
            var secretFromAzureAppConfiguration = configuration["postechcadpacapi:repositorysettings:secret"];
            string connectionString = string.Format(config.ConnectionString, secretFromAzureAppConfiguration ?? config.Secret);


            //  mongodb+srv://alrtechcld:s9sOTHMSCUVNsbaJ@mongocluster.3oa3jww.mongodb.net/

            var mongoClient = new MongoClient(connectionString);
            var mongoDataBase = mongoClient.GetDatabase(config.DataBase);

            _database = mongoDataBase.GetCollection<PacienteDataModel>(config.RepositoryName);
            _pacienteConverter = pacienteConverter;
            _pacienteModelConverter = pacienteModelConverter;
            _configuration = configuration;
        }

        public void Delete(string id)
        {
            var filter = Builders<PacienteDataModel>.Filter.Eq(g => g.Id, id);
            var result = _database.DeleteOne(filter);
        }

        public List<Paciente> FindByName(string name)
        {
            var dbItens = _database.Find(a => a.Nome.Contains(name)).ToList();
            return dbItens.Select(e => _pacienteModelConverter.Convert(e)).ToList();
        }

        public List<Paciente> GetAll()
        {
            var dbItens =  _database.Find(a => true).ToList();
            return dbItens.Select(e => _pacienteModelConverter.Convert(e)).ToList();
         }

        public Paciente GetById(string id)
        {
            var dbItem = _database.Find(a => a.Id == id).FirstOrDefault();
            return dbItem != null ? _pacienteModelConverter.Convert(dbItem) : null;
        }

        public Paciente UpSert(Paciente paciente)
        {
            var pacienteModel = _pacienteConverter.Convert(paciente);
            if (string.IsNullOrEmpty(pacienteModel.Id))
                _database.InsertOne(pacienteModel);
            else
                _database.ReplaceOne(p => p.Id == pacienteModel.Id, pacienteModel);

            return _pacienteModelConverter.Convert(pacienteModel);
        }
    }
}