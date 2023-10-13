import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { OrderByPipe } from './order-by.pipe';
import { QuickAccessMenuComponent } from '../shared/quick-access-menu/quick-access-menu.component';
import { RouterModule } from '@angular/router';
import {ButtonModule} from 'primeng/button';


@NgModule({
  declarations: [
    OrderByPipe,
    QuickAccessMenuComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    RouterModule,
    ButtonModule
  ],
  exports: [
    OrderByPipe,
    FormsModule,
    HttpClientModule,
    QuickAccessMenuComponent
  ]
})
export class SharedModule { }
