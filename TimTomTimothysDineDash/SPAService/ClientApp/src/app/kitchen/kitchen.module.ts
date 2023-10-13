import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { KitchenComponent } from '../kitchen/kitchen.component';
import { StartCookingOrderComponent } from './start-cooking-order/start-cooking-order.component';
import { MarkOrderFinishedComponent } from './mark-order-finished/mark-order-finished.component';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from 'primeng/api';
import { HttpClientModule } from '@angular/common/http';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { MessagesModule } from 'primeng/messages';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { SelectButtonModule } from 'primeng/selectbutton';
import { SliderModule } from 'primeng/slider';
import { ToastModule } from 'primeng/toast';



@NgModule({
  declarations: [
    KitchenComponent,
    StartCookingOrderComponent,
    MarkOrderFinishedComponent
  ],
  imports: [
    SharedModule,
    CommonModule,
    FormsModule,
    RouterModule.forChild([
      { 
        path: 'Kitchen', 
        component: KitchenComponent,
        children: [
          { path: 'MarkOrderFinished', component: MarkOrderFinishedComponent},
          { path: 'StartCookingOrder', component: StartCookingOrderComponent},
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
  ]
})
export class KitchenModule { }
