import { Injectable, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { InsertTakenOrderComponent } from './insert-taken-order/insert-taken-order.component';

@Injectable({
  providedIn: 'root'
})
export class PreventOrderLossGuard implements CanDeactivate<unknown> {
  
  canDeactivate(
    component: InsertTakenOrderComponent,
    currentRoute: ActivatedRouteSnapshot,
    currentState: RouterStateSnapshot,
    nextState?: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    
    return component.isFormDirty()
    ? window.confirm('You may lose the data you have in the order form if you proceed. Do you want to continue regardless?')
    : true;
  }

}
