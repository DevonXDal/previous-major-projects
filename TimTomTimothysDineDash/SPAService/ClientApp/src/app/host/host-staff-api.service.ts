import { HttpClient, HttpErrorResponse, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, lastValueFrom, of, tap, throwError } from 'rxjs';
import { HostStaffStatus } from './data/host-staff-status';
import { Constants } from '../constants';

@Injectable({
  providedIn: 'root'
})
export class HostStaffApiService {
  _currStatus?: HostStaffStatus; // This will always reflect whether the recent data came in
  private httpFormOptions: Object = { // Must be typed to Object
    observe: 'response',
    responseType: 'text'
  };

  public get currStatus() {
    return this._currStatus;
  }

  constructor(private http: HttpClient) { }

  fetchStatus(): Observable<HostStaffStatus> {
    const url = `${Constants.apiRoot}/${Constants.apiVersion}/HostStaff/Status`

    return this.http.get<HostStaffStatus>(url)
      .pipe(
        tap(data => this._currStatus = data),
        catchError(err => this.handleError(err))
      );
  }

  async insertPartyIntoQueue(numInParty: number): Promise<string> {
    const url = `${Constants.apiRoot}/${Constants.apiVersion}/HostStaff/DeclareNewCustomersInQueue`
    
    
    if (numInParty <= 0 || numInParty > 6) {
      return Promise.resolve('A party entering the queue must have 1-6 people');
    }

    let formData = new FormData();
    let response: HttpErrorResponse | HttpResponse<any> | null = null;
    formData.append('numOfCustomers', numInParty.toString());

    try {
      response = await lastValueFrom(this.http.put<HttpResponse<any> | HttpErrorResponse>(url, formData, this.httpFormOptions), {defaultValue: null})
    } catch (err) {
      response = err as HttpErrorResponse;
    }
    
    if (!response) return Promise.resolve(Constants.noResponseFromApi);
    if (response.status == 200) return Promise.resolve('');
    return Promise.resolve('Failed to add the customers to the queue'); // Fail message
  }

  async seatAtTable(tableId: number): Promise<string> {
    const url = `${Constants.apiRoot}/${Constants.apiVersion}/HostStaff/SeatNextGroupAtTable`

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
    
    return Promise.resolve('Failed to seat the next group of customers at the table'); // Fail message
  }


  private handleError(err: any): Observable<never> {
    let errorMessage: string;
    if (err.error instanceof ErrorEvent) {
      // A client-side or network error occurred
      errorMessage = `An error occurred: ${err.error.message}`;
    } else {
      // The backend returned an unsuccessful response code.
      errorMessage = `Backend returned code ${err.status}: ${err.body.error} for Host Staff`;
    }
    console.error(err);
    return throwError(() => errorMessage);
  }
}
