using NLog;
using System;
using System.Text;
using Thorium.Shared;
using System.Net;
using System.Net.Sockets;
using Thorium.Shared.FunctionServer.Tcp;
using Thorium.Server.TcpApi.Functions;

namespace Thorium.Server.TcpApi
{
    public class ThoriumServerTcpApi
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly FunctionServerTcp api;

        public ThoriumServerTcpApi()
        {
            var apiInterface = Settings.Get<string>("tcpApiInterface");
            if(apiInterface == null)
            {
                throw new Exception("tcpApiInterface is null");
            }
            var apiListener = new TcpListener(IPAddress.Parse(apiInterface), Settings.Get<int>("tcpApiPort"));

            api = new FunctionServerTcp(apiListener, Encoding.ASCII.GetBytes("THOR"));

            api.FunctionCallHandler.AddFunctionProvider(new Register());
        }

        public void Start()
        {
            api.Start();
            logger.Info("TCP API listening on port " + Settings.Get<int>("tcpApiPort"));
        }

        /*TaskDTO GetNextTask(FunctionServerTcpClient client)
        {
            Thread.Sleep(5000);
            //TODO: this is a race condition waiting to happen
            foreach (var reader in Database.ExecuteQuery("SELECT id, job_id, `index`, status FROM tasks WHERE status = ? LIMIT 1", TaskStatus.Queued.ToString()))
            {
                var dto = new TaskDTO();
                dto.Id = reader.GetString(0);
                dto.JobId = reader.GetString(1);
                dto.TaskNumber = reader.GetInt32(2);
                dto.Status = TaskStatus.Queued;
                Database.ExecuteNonQuery("UPDATE tasks SET status = ? WHERE id = ?", TaskStatus.Running.ToString(), dto.Id);
                return dto;
            }
            return null;
        }


        JobDTO GetJob(FunctionServerTcpClient client, string id)
        {
            foreach (var jobReader in Database.ExecuteQuery("SELECT id, name, description FROM jobs WHERE id = ? LIMIT 1", id))
            {
                var dto = new JobDTO();
                dto.Id = jobReader.GetString(0);
                dto.Name = jobReader.GetString(1);
                dto.Description = jobReader.GetString(2);

                List<OperationDTO> operations = new List<OperationDTO>();

                foreach (var operationsReader in Database.ExecuteQuery("SELECT id,job_id,`index`, type FROM operations WHERE job_id =? ORDER BY `index` ASC", dto.Id))
                {
                    var opDto = new OperationDTO();
                    operations.Add(opDto);
                    opDto.Id = operationsReader.GetString(0);
                    opDto.OperationType = operationsReader.GetString(3);
                    switch (opDto.OperationType)
                    {
                        case "exe":
                            foreach (var opExeReader in Database.ExecuteQuery("SELECT op_id,file_path,arguments,working_dir FROM op_exes WHERE op_id=? LIMIT 1", opDto.Id))
                            {
                                var exeData = new ExeDTO();
                                opDto.OperationData = exeData;
                                exeData.FilePath = opExeReader.GetString(1);
                                var args = opExeReader.GetValue(2);
                                if (args is not DBNull)
                                {
                                    exeData.Arguments = (string[])args;
                                }
                                exeData.WorkingDir = opExeReader.GetValue(3) as string;
                            }
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                }

                dto.Operations = operations.ToArray();
                return dto;
            }
            return null;
        }

        void TurnInTask(FunctionServerTcpClient client, string taskId)
        {
            Database.ExecuteNonQuery("UPDATE tasks SET status = ? WHERE id = ?", TaskStatus.Finished.ToString(), taskId);
        }*/
    }
}
