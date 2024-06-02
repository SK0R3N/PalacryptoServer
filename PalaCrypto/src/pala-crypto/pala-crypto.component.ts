import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { CrosshairMode, createChart } from 'lightweight-charts';
import { lastValueFrom } from 'rxjs';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-pala-crypto',
  templateUrl: './pala-crypto.component.html',
  styleUrls: ['./pala-crypto.component.css']
})
export class PalaCryptoComponent {

  @ViewChild('chartContainer', { static: true }) chartContainer!: ElementRef;

  ApiPalacrypto : String = "https://localhost:7000/"
  listCandle : any
  listData : any = []
  listSearch : any = []
  selectedOption = 'redstone'
  chart : any
  time : string = "1"
  valuetime:string = "D"

  constructor(private http: HttpClient) { }

  async ngOnInit(): Promise<void> {
    await this.GetGraphData();
    await this.GetLastDifference()
    await this.GetAllLastDifference()

    this.chart = createChart(this.chartContainer.nativeElement, {
      layout: {
        textColor: '#d1d4dc',
        background: { color: '#161313' }
      },
      grid: {
        vertLines: {
          color: 'rgba(197, 203, 206, 0.5)',
        },
        horzLines: {
          color: 'rgba(197, 203, 206, 0.5)',
        },
      },
      crosshair: {
        mode: CrosshairMode.Normal,
      },
      timeScale: {
        borderColor: 'rgba(197, 203, 206, 1)',
      },
    });

    const candleSeries = this.chart.addCandlestickSeries({
      upColor: 'rgba(90, 156, 89, 1)',
      downColor: 'rgba(199, 77, 70, 1)',
      borderDownColor: 'rgba(199, 77, 70, 1)',
      borderUpColor: 'rgba(90, 156, 89, 1)',
      wickDownColor: 'rgba(199, 77, 70, 1)',
      wickUpColor: 'rgba(90, 156, 89, 1)',
    });

    candleSeries.setData(this.listCandle);


  }

  async updatetime(time:string, timevalue:string) {


    this.time =time;
    this.valuetime = timevalue
    this.reload(this.selectedOption)
  }

  async reload(value: string) {
    this.selectedOption = value
    await this.GetGraphData();
    await this.GetLastDifference()
    await this.GetAllLastDifference()

    this.chart.remove();
    this.chart = null;

    this.chart = createChart(this.chartContainer.nativeElement, {
      layout: {
        textColor: '#d1d4dc',
        background: { color: '#161313' }
      },
      grid: {
        vertLines: {
          color: 'rgba(197, 203, 206, 0.5)',
        },
        horzLines: {
          color: 'rgba(197, 203, 206, 0.5)',
        },
      },
      crosshair: {
        mode: CrosshairMode.Normal,
      },
      timeScale: {
        borderColor: 'rgba(197, 203, 206, 1)',
      },
    });

    const candleSeries = this.chart.addCandlestickSeries({
      upColor: 'rgba(90, 156, 89, 1)',
      downColor: 'rgba(199, 77, 70, 1)',
      borderDownColor: 'rgba(199, 77, 70, 1)',
      borderUpColor: 'rgba(90, 156, 89, 1)',
      wickDownColor: 'rgba(199, 77, 70, 1)',
      wickUpColor: 'rgba(90, 156, 89, 1)',
    });

    candleSeries.setData(this.listCandle);


  }

  async GetLastDifference() {
    
    let data = await lastValueFrom(this.http.get<any>(this.ApiPalacrypto +"PalaCrypto/GetAllPriceItem/"+ this.selectedOption))
    this.listData = [];
    console.log(data.result)
    for(let [cle,valeur] of Object.entries(data.result))
    {
      let x = data.result[cle]

      const difference = x.newPrice - x.lastPrice;
      let pourcentage = (difference /  x.lastPrice) * 100;
      var pourcentagestring = pourcentage.toFixed(2)
      var t = new log(x.newPrice, x.lastPrice, pourcentagestring)

      this.listData.push(t)
      
    }
  }

  async GetLastDifferencePositive() {
   
    let data = await lastValueFrom(this.http.get<any>(this.ApiPalacrypto +"PalaCrypto/GetAllPriceItemPositive/"+ this.selectedOption))
    this.listData = [];
    console.log(data.result)
    for(let [cle,valeur] of Object.entries(data.result))
    {
      let x = data.result[cle]

      const difference = x.newPrice - x.lastPrice;
      let pourcentage = (difference /  x.lastPrice) * 100;
      var pourcentagestring = pourcentage.toFixed(2)
      var t = new log(x.newPrice, x.lastPrice, pourcentagestring)

      this.listData.push(t)
      
    }
  }

  async GetLastDifferenceNegative() {
    
    let data = await lastValueFrom(this.http.get<any>(this.ApiPalacrypto +"PalaCrypto/GetAllPriceItemNegative/"+ this.selectedOption))
    this.listData = [];
    console.log(data.result)
    for(let [cle,valeur] of Object.entries(data.result))
    {
      let x = data.result[cle]

      const difference = x.newPrice - x.lastPrice;
      let pourcentage = (difference /  x.lastPrice) * 100;
      var pourcentagestring = pourcentage.toFixed(2)
      var t = new log(x.newPrice, x.lastPrice, pourcentagestring)

      this.listData.push(t)
      
    }
  }

  async GetAllLastDifference() {
    let data = await lastValueFrom(this.http.get<any>(this.ApiPalacrypto +"PalaCrypto/GetAllLastDifference"))
    this.listSearch = [];
    console.log(data.result)
    for(let [cle,valeur] of Object.entries(data.result))
    {
      let x = data.result[cle]

      const difference = x.newPrice - x.lastPrice;
      let pourcentage = (difference /  x.lastPrice) * 100;
      var pourcentagestring = pourcentage.toFixed(2)
      var t = new itemSearch(x.nameItem,x.lastPrice, x.newPrice, pourcentagestring)

      this.listSearch.push(t)
      
    }
  }


  async GetGraphData() {
    this.listCandle = []

    let data = await lastValueFrom(this.http.get<any>(this.ApiPalacrypto + "PalaCrypto/GetPriceForChart/"+ this.selectedOption + "/" + this.valuetime + "/" + this.time))
    console.log(data)
    console.log(data.result)
    let x = data.result
    for(let [cle, valeur] of Object.entries(x))
      {
        let t = data.result[cle]
        let date = new Date(t.logTime);
        let timestamp = date.getTime();
        if(t.lastPrice > t.newPrice)
          { 
              let high = t.lastPrice + (t.lastPrice * this.generateRandomNumber() )
              let low = t.lastPrice - (t.lastPrice * this.generateRandomNumber() )
              this.listCandle.push({ time: Math.floor(timestamp / 1000) , open: t.lastPrice, high: high, low: low, close: t.newPrice})
          }
          else if(t.lastPrice < t.newPrice)
            { 
                let high = t.lastPrice - (t.lastPrice * this.generateRandomNumber() )
                let low = t.lastPrice + (t.lastPrice * this.generateRandomNumber() )
                this.listCandle.push({ time: Math.floor(timestamp / 1000) , open: t.lastPrice, high: high, low: low, close: t.newPrice})
            }
      }
  }

  generateRandomNumber(): number {
    const min = 0.0002;
    const max = 0.0005;
    return Math.random() * (max - min) + min;
}

}

class log {
  constructor(public newPrice: number , public lastPrice: number, public pourcentage : string) {
    
  }
} 

class itemSearch {
  constructor(public item: string,public lastPrice: number,public newPrice: number, public pourcentage : string) {
    
  }
} 


