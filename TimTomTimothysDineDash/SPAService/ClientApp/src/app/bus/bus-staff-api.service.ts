import { Injectable } from '@angular/core';
import { BusStaffStatus } from './data/bus-staff-status';
import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Observable, tap, catchError, lastValueFrom, throwError } from 'rxjs';
import { Constants } from '../constants';

@Injectable({
  providedIn: 'root'
})
export class BusStaffApiService {

  _currStatus?: BusStaffStatus; // This will always reflect whether the recent data came in
  private httpFormOptions: Object = { // Must be typed to Object
    observe: 'response',
    responseType: 'text'
  };

  public get currStatus() {
    return this._currStatus;
  }

  constructor(private http: HttpClient) { }

  fetchStatus(): Observable<BusStaffStatus> {
    const url = `${Constants.apiRoot}/${Constants.apiVersion}/BusStaff/Status`

    return this.http.get<BusStaffStatus>(url)
      .pipe(
        tap(data => this._currStatus = data),
        catchError(err => this.handleError(err))
      );
  }

  async markTableAsCleanAndReady(tableId: number): Promise<string> {
    const url = `${Constants.apiRoot}/${Constants.apiVersion}/BusStaff/MarkTableAsCleanedForTheNextGroup`

    let formData = new FormData();
    let response: HttpErrorResponse | HttpResponse<any> | null = null;
    formData.append('tableId', tableId.toString());

    try {
      response = await lastValueFrom(this.http.post<HttpResponse<any> | HttpErrorResponse>(url, formData, this.httpFormOptions), {defaultValue: null})
    } catch (err) {
      response = err as HttpErrorResponse;
    }
    
    
    if (!response) return Promise.resolve(Constants.noResponseFromApi);
    if (response.status == 200) return Promise.resolve('');
    
    return Promise.resolve(`Failed to mark table ${tableId} as cleaned and prepared`); // Fail message
  }

  private handleError(err: any): Observable<never> {
    let errorMessage: string;
    if (err.error instanceof ErrorEvent) {
      // A client-side or network error occurred
      errorMessage = `An error occurred: ${err.error.message}`;
    } else {
      // The backend returned an unsuccessful response code.
      errorMessage = `Backend returned code ${err.status}: ${err.body.error} for Bus Staff`;
    }
    console.error(err);
    return throwError(() => errorMessage);
  }
}
