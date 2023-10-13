import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InsertTakenOrderComponent } from './insert-taken-order/insert-taken-order.component';
import { MarkOrderPaidComponent } from './mark-order-paid/mark-order-paid.component';
import { RouterModule } from '@angular/router';
import { WaitComponent } from './wait.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { SharedModule } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { MessagesModule } from 'primeng/messages';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { SelectButtonModule } from 'primeng/selectbutton';
import { SliderModule } from 'primeng/slider';
import { ToastModule } from 'primeng/toast';
import {InputTextareaModule} from 'primeng/inputtextarea';
import { PreventOrderLossGuard } from './prevent-order-loss.guard';


@NgModule({
  declarations: [
    WaitComponent,
    InsertTakenOrderComponent,
    MarkOrderPaidComponent
  ],
  imports: [
    SharedModule,
    CommonModule,
    FormsModule,
    RouterModule.forChild([
      {
        path: 'Wait', 
        component: WaitComponent,
        children: [
          {path: 'InsertTakenOrder', component: InsertTakenOrderComponent, canDeactivate: [PreventOrderLossGuard]},
          {path: 'MarkOrderPaid', component: MarkOrderPaidComponent}
        ]
      },  
    ]),
    ButtonModule,
    ProgressSpinnerModule,
    SelectButtonModule,
    SliderModule,
    ToastModule,
    MessagesModule,
    DropdownModule,
    HttpClientModule,
    InputTextareaModule
  ]
})
export class WaitModule { }
