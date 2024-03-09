export interface ShortenUrlRequest{
  longUrl:string,
  remainingClicks:number|null,
  expiresInMinutes:number|null
}
