import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Constants } from './constants';
import { lastValueFrom, timeout } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class HealthCheckApiService {
  private httpFormOptions: Object = { // Must be typed to Object
    observe: 'response',
    responseType: 'text'
  };

  constructor(private http: HttpClient) { }

  async checkApiReachable(): Promise<boolean> {
    const url = `${Constants.apiRoot}/Is-Alive`

    try {
      let response = await lastValueFrom(this.http.get<HttpResponse<any>>(url, this.httpFormOptions).pipe(timeout(3000)), {defaultValue: null})
    
      if (!response) return Promise.resolve(false);
      if (response.status == 200) return Promise.resolve(true);
      
      // Do nothing
    } catch (_) {
      // Do nothing
    } 
    return Promise.resolve(false); // If there is a non-200 or some exception is thrown, assume no connection is available
  }
}
