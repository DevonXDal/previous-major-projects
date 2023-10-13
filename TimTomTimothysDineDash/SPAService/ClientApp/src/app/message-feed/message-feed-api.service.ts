import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Messages } from './messages';
import { Constants } from '../constants';
import { Observable, tap, timeout } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MessageFeedApiService {
  constructor(private http: HttpClient) { }

  fetchStatus(): Observable<Messages> {
    const url = `${Constants.apiRoot}/${Constants.apiVersion}/MessageFeed/Status`

    return this.http.get<Messages>(url)
      .pipe(
        timeout(3500), // 3.5 seconds to retrieve time limit
      );
  }
}
