import { trigger, animate, transition, style, group, query } from '@angular/animations';

export const slideInAnimation = trigger('slideInAnimation', [
  // Transition between any two states
  transition('* <=> *', [
    // Events to apply
    // Defined style and animation function to apply
    // Config object with optional set to true to handle when element not yet added to the DOM
    query(':enter, :leave', style({ position: 'fixed', width: '100%', zIndex: 2 }), { optional: true }),
    // group block executes in parallel
    group([
      query(':enter', [
        style({ opacity: '0', width: 'fit-content'} ),
        animate('3.5s ease-in', style({ opacity: '100' }))
      ], { optional: true }),
      query(':leave', [
        style({ opacity: '100', width: 'fit-content'}),
        animate('3.5s ease-in', style({ opacity: '0' }))
      ], { optional: true })
    ])
  ])
]);