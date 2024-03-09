import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {MyConfig} from "../my-config";
import {ShortenUrlRequest} from "../models/shorten-url-request";

@Injectable({
  providedIn:"root"
})

export class UrlShortenerService {
  serviceUrl:string = "";
  constructor(private httpClient:HttpClient) {
    this.serviceUrl = MyConfig.server + "/shorten";
  }

  getShortUrl(request:ShortenUrlRequest){
    return this.httpClient.post(this.serviceUrl, request);
  }
}
