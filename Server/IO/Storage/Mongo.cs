using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatServer.Entity;
using ChatServer.Entity.Meta.Profile;

namespace ChatServer.IO.Storage
{
    public class Mongo
    {
        private IMongoDatabase db;
        private MongoClient client;

        private int TryLimit = 10;
        private int TryCount = 0;

        public delegate T Query<T>(IMongoCollection<T> collection);
        public delegate void ExecuteNonQuery<T>(IMongoCollection<T> collection);

        public static readonly string DatabaseName = "SimpleChat";
        public static readonly string UserCollectionName = "Users";
        public static readonly string RelationCollectionName = "Relations";
        public static readonly string FileMapName = "FileMap";

        private Mongo()
        {
            Init();
            RegisterClassMap();
        }

        private void RegisterClassMap()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(ChatUser)))
            {
                BsonClassMap.RegisterClassMap<ChatUser>();
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(ChatUserProfile)))
            {
                BsonClassMap.RegisterClassMap<ChatUserProfile>();
            }
        }

        private void Init()
        {
            client = new MongoClient();
            db = client.GetDatabase(DatabaseName);
            var collection = db.GetCollection<ChatUser>(UserCollectionName);
            collection.Indexes.CreateOneAsync(Builders<ChatUser>.IndexKeys.Ascending(p => p.Email), new CreateIndexOptions(){Unique = true}).Wait();
        }

        public T Get<T>(string collectionName, Query<T> executable)
        {
            T result = default(T);

            try
            {
                var collection = db.GetCollection<T>(collectionName);
                result = executable(collection);
            } catch (MongoConnectionClosedException e)
            {
                if (TryLimit < TryCount) return result;
                TryCount++;
                SimpleChatServer.GetServer().Logger.Error(e);
                client = new MongoClient();
                db = client.GetDatabase(DatabaseName);

                result = Get<T>(collectionName, executable);
            } 
            catch (Exception e)
            {
                SimpleChatServer.GetServer().Logger.Error(e);
            }
            TryCount = 0;

            return result;
        }

        public void Set<T>(string collectionName, ExecuteNonQuery<T> executable)
        {
            try
            {
                var collection = db.GetCollection<T>(collectionName);
                executable(collection);
            }
            catch (MongoConnectionClosedException e)
            {
                if (TryLimit < TryCount) return;
                TryCount++;
                SimpleChatServer.GetServer().Logger.Error(e);
                client = new MongoClient();
                db = client.GetDatabase(DatabaseName);

                Set<T>(collectionName, executable);
            }
            catch (Exception e)
            {
                SimpleChatServer.GetServer().Logger.Error(e);
            }
            TryCount = 0;
        }

        public static void StartService(bool forceRestart = false)
        {
            try
            {
                if (Instance == null || forceRestart)
                {
                    Instance = new Mongo();
                    SimpleChatServer.GetServer().Logger.Info("MongoDB Service started successfully");
                }
            } catch (Exception e)
            {
                SimpleChatServer.GetServer().Logger.Error(e);
            }
        }

        public static Mongo Instance { get; private set; }
    }
}
