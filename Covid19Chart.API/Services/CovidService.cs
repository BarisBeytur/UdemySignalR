using Covid19Chart.API.Hubs;
using Covid19Chart.API.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Covid19Chart.API.Services
{
    public class CovidService
    {
        private readonly AppDbContext _dbContext;
        private readonly IHubContext<CovidHub> _hubContext;

        public CovidService(AppDbContext dbContext, IHubContext<CovidHub> hubContext)
        {
            _dbContext = dbContext;
            _hubContext = hubContext;
        }

        public IQueryable<Covid> GetList()
        {
            return _dbContext.Covids.AsQueryable();
        }

        public async Task SaveCovid(Covid covid)
        {
            await _dbContext.Covids.AddAsync(covid);
            await _dbContext.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("ReceiveCovidList", GetCovidChartList());
        }

        public List<CovidChart> GetCovidChartList()
        {
            List<CovidChart> covidCharts = new List<CovidChart>();
            using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "select tarih,[1],[2],[3],[4],[5] from\r\n(select [City],[Count],Cast([CovidDate] as date)as tarih FROM Covids) as covidT\r\nPIVOT\r\n(SUM(Count) \r\nFOR\r\nCity IN([1],[2],[3],[4],[5])) as PTable \r\norder by tarih asc";

                command.CommandType = System.Data.CommandType.Text;

                _dbContext.Database.OpenConnection();

                using (var reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        CovidChart cc = new CovidChart();
                        cc.CovidDate = reader.GetDateTime(0).ToShortDateString();
                        foreach (var item in Enumerable.Range(1, 5).ToList())
                        {
                            if (System.DBNull.Value.Equals(reader[item]))
                            {
                                cc.Counts.Add(0);
                            }
                            else
                            {
                                cc.Counts.Add(reader.GetInt32(item));
                            }
                        }
                        covidCharts.Add(cc);
                    }
                }

                _dbContext.Database.CloseConnection();

                return covidCharts;
            }
        }

    }
}
