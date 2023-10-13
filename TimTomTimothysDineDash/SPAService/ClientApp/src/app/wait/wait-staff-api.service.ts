import { Injectable } from '@angular/core';
import { WaitStaffStatus } from './data/wait-staff-status';
import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Observable, tap, catchError, throwError, lastValueFrom } from 'rxjs';
import { Constants } from '../constants';
import { HostStaffStatus } from '../host/data/host-staff-status';

@Injectable({
  providedIn: 'root'
})
export class WaitStaffApiService {

  _currStatus?: WaitStaffStatus; // This will always reflect whether the recent data came in
  private httpFormOptions: Object = { // Must be typed to Object
    observe: 'response',
    responseType: 'text'
  };

  public get currStatus() {
    return this._currStatus;
  }

  constructor(private http: HttpClient) { }

  fetchStatus(): Observable<WaitStaffStatus> {
    const url = `${Constants.apiRoot}/${Constants.apiVersion}/WaitStaff/Status`

    return this.http.get<WaitStaffStatus>(url)
      .pipe(
        tap(data => this._currStatus = data),
        catchError(err => this.handleError(err))
      );
  }
  
  async insertTakenOrder(tableId: number, orderDescription: string): Promise<string> {
    const url = `${Constants.apiRoot}/${Constants.apiVersion}/WaitStaff/InsertOrderForTable`

    let formData = new FormData();
    let response: HttpErrorResponse | HttpResponse<any> | null = null;
    formData.append('tableId', tableId.toString());
    formData.append('orderDescription', orderDescription);

    try {
      response = await lastValueFrom(this.http.put<HttpResponse<any> | HttpErrorResponse>(url, formData, this.httpFormOptions), {defaultValue: null})
    } catch (err) {
      response = err as HttpErrorResponse;
    }
    
    
    if (!response) return Promise.resolve(Constants.noResponseFromApi);
    if (response.status == 200) return Promise.resolve('');
    
    return Promise.resolve(`Failed to insert an order for table ${tableId}`); // Fail message
  }

  async markTableAsHavingPaid(tableId: number): Promise<string> {
    const url = `${Constants.apiRoot}/${Constants.apiVersion}/WaitStaff/MarkTableAsHavingPaidAndLeft`

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
    
    return Promise.resolve(`Failed to to mark table ${tableId} as having paid and left`); // Fail message
  }

  private handleError(err: any): Observable<never> {
    let errorMessage: string;
    if (err.error instanceof ErrorEvent) {
      // A client-side or network error occurred
      errorMessage = `An error occurred: ${err.error.message}`;
    } else {
      // The backend returned an unsuccessful response code.
      errorMessage = `Backend returned code ${err.status}: ${err.body.error} for Wait Staff`;
    }
    console.error(err);
    return throwError(() => errorMessage);
  }
}
