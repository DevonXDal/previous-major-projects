import { NgModule } from '@angular/core';
import { HostComponent } from '../host/host.component';
import { AddToWaitQueueComponent } from './add-to-wait-queue/add-to-wait-queue.component';
import { SeatATableComponent } from './seat-atable/seat-atable.component';
import { RouterModule } from '@angular/router';
import {SelectButtonModule} from 'primeng/selectbutton';
import {SliderModule} from 'primeng/slider';
import {ProgressSpinnerModule} from 'primeng/progressspinner';
import {ToastModule} from 'primeng/toast';
import {MessagesModule} from 'primeng/messages';
import { SharedModule } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import {DropdownModule} from 'primeng/dropdown';
import { CommonModule } from '@angular/common';



@NgModule({
  declarations: [
    HostComponent,
    AddToWaitQueueComponent,
    SeatATableComponent
  ],
  imports: [
    SharedModule,
    CommonModule,
    FormsModule,
    RouterModule.forChild([
      { 
        path: 'Host', 
        component: HostComponent,
        children: [
          { path: 'AddToWaitQueue', component: AddToWaitQueueComponent},
          { path: 'SeatATable', component: SeatATableComponent}
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
    HttpClientModule
  ]
})
export class HostModule { }
