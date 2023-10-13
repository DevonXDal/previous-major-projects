import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MarkTableAsCleanedComponent } from './mark-table-as-cleaned/mark-table-as-cleaned.component';
import { RouterModule } from '@angular/router';
import { BusComponent } from './bus.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { MessagesModule } from 'primeng/messages';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { SelectButtonModule } from 'primeng/selectbutton';
import { SliderModule } from 'primeng/slider';
import { ToastModule } from 'primeng/toast';
import { SharedModule } from '../shared/shared.module';



@NgModule({
  declarations: [
    BusComponent,
    MarkTableAsCleanedComponent
  ],
  imports: [
    SharedModule,
    CommonModule,
    FormsModule,
    RouterModule.forChild([
      {
        path: 'Bus', 
        component: BusComponent,
        children: [
          {path: 'MarkTableAsCleaned', component: MarkTableAsCleanedComponent}
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
export class BusModule { }
