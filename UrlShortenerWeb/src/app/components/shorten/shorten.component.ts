import {Component, Inject} from '@angular/core';
import { Clipboard } from "@angular/cdk/clipboard";
import {UrlShortenerService} from "../../services/url-shortener-service";
import {ShortenUrlRequest} from "../../models/shorten-url-request";
import {ShortenUrlResponse} from "../../models/shorten-url-response";
import {FormsModule} from "@angular/forms";
import {ErrorResponse} from "../../models/errorResponse";
import {CommonModule} from "@angular/common";

@Component({
  selector: 'app-shorten',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
  ],
  templateUrl: './shorten.component.html',
  styleUrl: './shorten.component.css'
})
export class ShortenComponent {
  constructor(private clipboard: Clipboard,
              @Inject(UrlShortenerService) private urlShortenerService : UrlShortenerService) {

  }

  copiedToClipboard:boolean=false;
  generating:boolean=false;

  expiryNumber:number|null = null;
  expiryTime:string|null = null;

  shortenUrlRequest:ShortenUrlRequest={
    longUrl:"",
    remainingClicks:null,
    expiresInMinutes:null
  }

  shortenUrlResponse:ShortenUrlResponse={
    shortUrl:""
  }

  errorResponse:ErrorResponse|null=null;
  errorMessage:string="";

  getShortUrl() {
    this.generating=true;
    this.copiedToClipboard = false;
    this.shortenUrlResponse.shortUrl="";
    this.errorMessage="";

    this.expiryTime = this.expiryTime == "" ? null : this.expiryTime;
    if (this.expiryNumber != null && this.expiryTime != null) {
      this.shortenUrlRequest.expiresInMinutes = this.expiryNumber * Number.parseInt(this.expiryTime);
    } else {
      this.shortenUrlRequest.expiresInMinutes = null;
    }

    this.urlShortenerService.getShortUrl(this.shortenUrlRequest).subscribe((x: any) => {
      this.shortenUrlResponse = x;
      this.generating=false;
    }, error => {
      this.errorResponse = error.error;
      this.errorResponse!.errors.forEach((error: string) => {
        this.errorMessage += error + "<br>";
        this.generating=false;
      });
    })
  }

  copyToClipboard() {
    const textToCopy = document.querySelector('.form-control.user-select-all')?.textContent;
    if (textToCopy) {
      this.clipboard.copy(textToCopy);
      this.copiedToClipboard = true;
    }
  }
}
