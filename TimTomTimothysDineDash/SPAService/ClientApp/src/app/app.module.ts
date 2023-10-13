import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '../environments/environment';

// PrimeNG
import {GalleriaModule} from 'primeng/galleria'
import {SelectButtonModule} from 'primeng/selectbutton';
import {SliderModule} from 'primeng/slider';
import {ProgressSpinnerModule} from 'primeng/progressspinner';
import {ToastModule} from 'primeng/toast';
import {MessagesModule} from 'primeng/messages';
import {ButtonModule} from 'primeng/button';


import { WaitModule } from './wait/wait.module';
import { HostModule } from './host/host.module';
import { BusModule } from './bus/bus.module';
import { KitchenModule } from './kitchen/kitchen.module';
import { MessageFeedComponent } from './message-feed/message-feed.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MessageService } from 'primeng/api';
import { SharedModule } from './shared/shared.module';
import { TimsOverlayService } from './shared/tims-overlay.service';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    MessageFeedComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserAnimationsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full', outlet: 'primary' },
      { path: 'counter', component: CounterComponent, outlet: 'primary' },
      { path: 'fetch-data', component: FetchDataComponent, outlet: 'primary' },
      { path: '', component: MessageFeedComponent, outlet: 'secondary' }
    ]),
    ServiceWorkerModule.register('ngsw-worker.js', {
      enabled: environment.production,
      // Register the ServiceWorker as soon as the application is stable
      // or after 30 seconds (whichever comes first).
      registrationStrategy: 'registerWhenStable:30000'
    }),
    GalleriaModule,
    WaitModule,
    HostModule,
    BusModule,
    KitchenModule,
    ButtonModule,
    ProgressSpinnerModule,
    SelectButtonModule,
    SliderModule,
    ToastModule,
    MessagesModule,
    SharedModule,
  ],
  providers: [MessageService, TimsOverlayService],
  bootstrap: [AppComponent]
})
export class AppModule { }
