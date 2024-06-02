using Microsoft.EntityFrameworkCore;
using PalaCrypto.DbContextPalacryto;
using PalaCrypto.Model;
using System.ComponentModel;
using System.Text.Json;

namespace PalaCrypto.Service
{
    public class LogService
    {
        private readonly HttpClient _httpClient;
        private string APIPaladium = "https://api.paladium.games/v1/paladium";
        private readonly DbContextPalacrypto _context;

        public LogService()
        {

        }

        public LogService(IHttpClientFactory httpClientFactory, DbContextPalacrypto context)
        {
            _httpClient = httpClientFactory.CreateClient();
            _context = context;
        }

        public async Task<Dictionary<string,LogDifferenceAdmin>> GetPriceForChart(char letter, int time, string item)
        {
            switch (letter) {
                case 'm':
                    return await _context.LogDifferenceAdmins.Where(p => p.nameItem == item && p.logTime >= DateTime.Now.AddMinutes(-time)).ToDictionaryAsync(p => p.logTime.Minute.ToString(), p => p);
                case 'h':
                    return await _context.LogDifferenceAdmins.Where(p => p.nameItem == item && p.logTime >= DateTime.Now.AddHours(-time)).ToDictionaryAsync(p => $"{p.logTime.Hour}-{p.logTime.Minute}" , p => p);                
                case 'D':

                    var startDate = DateTime.Now.AddDays(- time);
                    var logs = await _context.LogDifferenceAdmins
                        .Where(log => log.nameItem == item && log.logTime >= startDate)
                        .ToListAsync();

                    var groupedLogs = logs
                        .GroupBy(log => new { log.logTime.Day, log.logTime.Hour })
                        .Select(g => new
                        {
                            LastLog = g.OrderBy(log => log.logTime).LastOrDefault(),
                            DayHourKey = $"{g.Key.Day}-{g.Key.Hour}"
                        })
                        .OrderBy(g => g.LastLog.logTime.Day)
                        .ThenBy(g => g.LastLog.logTime.Hour)
                        .ToDictionary(
                            g => g.DayHourKey,
                            g => new LogDifferenceAdmin
                            {
                                nameItem = item,
                                logTime = g.LastLog.logTime,
                                newPrice = g.LastLog.newPrice,
                                lastPrice = g.LastLog.lastPrice
                            });
                    return groupedLogs;

                case 'W':

                    var startWeek = DateTime.Now.AddDays(-time * 7);

                    return await _context.LogDifferenceAdmins
                    .Where(log => log.nameItem == item && log.logTime >= startWeek)
                    .GroupBy(log => new { log.logTime.Day, log.logTime.Hour })
                    .Select(g => new
                    {
                        DayHourKey = $"{g.Key.Day}-{g.Key.Hour}",
                        MaxNewPrice = g.Max(log => log.newPrice),
                        MinLastPrice = g.Min(log => log.lastPrice),
                        Time = g.Max(log => log.logTime)
                    })
                    .ToDictionaryAsync(
                        g => g.DayHourKey,
                        g => new LogDifferenceAdmin
                        {
                            nameItem = item,
                            logTime = g.Time,
                            newPrice = g.MaxNewPrice,
                            lastPrice = g.MinLastPrice
                        });
                case 'M':

                    var startMonth = DateTime.Now.AddMonths(-time );

                    return await _context.LogDifferenceAdmins
                    .Where(log => log.nameItem == item && log.logTime >= startMonth)
                    .GroupBy(log => new { log.logTime.Month, log.logTime.Day})
                    .Select(g => new
                    {
                        DayHourKey = $"{g.Key.Month}-{g.Key.Day}",
                        MaxNewPrice = g.Max(log => log.newPrice),
                        MinLastPrice = g.Min(log => log.lastPrice),
                        Time = g.Max(log => log.logTime)
                    })
                    .ToDictionaryAsync(
                        g => g.DayHourKey,
                        g => new LogDifferenceAdmin
                        {
                            nameItem = item,
                            logTime = g.Time,
                            newPrice = g.MaxNewPrice,
                            lastPrice = g.MinLastPrice
                        });
                case 'Y':
                    var startYear = DateTime.Now.AddYears(-time);
                    
                    var test  = await _context.LogDifferenceAdmins
                    .Where(log => log.nameItem == item && log.logTime >= startYear)
                    .GroupBy(log => new { log.logTime.Month, log.logTime.Day })
                    .Select(g => new
                    {
                        DayHourKey = $"{g.Key.Month}-{g.Key.Day}",
                        MaxNewPrice = g.Max(log => log.newPrice),
                        MinLastPrice = g.Min(log => log.lastPrice),
                        Time = g.Max(log => log.logTime)
                    })
                    .ToDictionaryAsync(
                        g => g.DayHourKey,
                        g => new LogDifferenceAdmin
                        {
                            nameItem = item,
                            logTime = g.Time,
                            newPrice = g.MaxNewPrice,
                            lastPrice = g.MinLastPrice
                        });
                    return test;
            }

            return await _context.LogDifferenceAdmins.Where(p => p.nameItem == item && p.logTime >= DateTime.Now.AddMinutes(-time)).ToDictionaryAsync(p => p.logTime.Minute.ToString(), p => p);
        }


        public async Task<List<LogDifferenceAdmin>> GetAllPriceItemPositive(string name)
        {
            List<LogDifferenceAdmin> log = await _context.LogDifferenceAdmins.Where(p => p.nameItem == name && (p.newPrice - p.lastPrice) > 0 && p.logTime >= DateTime.Now.AddDays(-3)).ToListAsync();

            return log;
        }

        public async Task<List<LogDifferenceAdmin>> GetAllPriceItemNegative(string name)
        {
            List<LogDifferenceAdmin> log = await _context.LogDifferenceAdmins.Where(p => p.nameItem == name && (p.newPrice - p.lastPrice) < 0 && p.logTime >= DateTime.Now.AddDays(-3)).ToListAsync();

            return log;
        }

        public async Task<List<LogDifferenceAdmin>> GetAllPriceItem(string name)
        {
            List<LogDifferenceAdmin> log = await _context.LogDifferenceAdmins.Where(p => p.nameItem == name && p.logTime >= DateTime.Now.AddDays(-3)).ToListAsync();

            return log;
        }

        public async Task<List<LogDifferenceAdmin>> GetAllLastDifference()
        {
            List<LogDifferenceAdmin> log = await _context.LogDifferenceAdmins.Where(p => p.logTime >= DateTime.Now.AddDays(-3)).ToListAsync();

            return log;
        }

        public async Task UpdateAdmin()
        {
            var response = await _httpClient.GetAsync(APIPaladium + "/shop/admin/items?limit=46");

            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            ApiResponse apiResponse = JsonSerializer.Deserialize<ApiResponse>(responseBody);

            List<LogAdminShop> list = apiResponse.data;
            

            foreach (var item in list)
            {
                LogAdmin lastLog = _context.LogAdmins.Where(i => i.nameItem == item.name).OrderBy(p => p.id).Last();

                if(lastLog.price != item.sellPrice)
                {
                    LogDifferenceAdmin logDifferenceAdmin = new LogDifferenceAdmin();

                    logDifferenceAdmin.lastPrice = lastLog.price;
                    logDifferenceAdmin.newPrice = item.sellPrice;
                    logDifferenceAdmin.nameItem = item.name;
                    logDifferenceAdmin.logTime = DateTime.Now;

                    _context.LogDifferenceAdmins.Add(logDifferenceAdmin);

                }

                LogAdmin Log = new LogAdmin();

                Log.price = item.sellPrice;
                Log.nameItem = item.name;
                Log.logTime = DateTime.Now;

                _context.LogAdmins.Add(Log);
                
            }
            _context.SaveChangesAsync();
        }
    }

    public class ApiResponse
    {
        public List<LogAdminShop> data { get; set; }
    }

    public class LogAdminShop
    {
        public string name { get; set; }
        public double buyPyrice { get; set; }
        public bool canSell { get; set; }
        public double sellPrice { get; set; }
        public string category { get; set; }
        public bool canBuy { get; set; }
    }
}
