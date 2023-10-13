import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  images: any[] = [
    {
        previewImageSrc: 'assets/img/slideshow/1.webp',
        thumbnailImageSrc: 'assets/img/slideshow/1.webp',
        alt: 'Logo: Finest quality 1-star dining',
        title: 'Dine&Dash Logo'
    },
    {
        previewImageSrc: 'assets/img/slideshow/2.webp',
        thumbnailImageSrc: 'assets/img/slideshow/2.webp',
        alt: 'Image of a bowl of ice cream',
        title: 'Ice Cream Bowl'
    },
    {
        previewImageSrc: 'assets/img/slideshow/3.webp',
        thumbnailImageSrc: 'assets/img/slideshow/3.webp',
        alt: 'Image of the boneless fried chicken meal',
        title: 'Boneless Fried Chicken Meal'
    },
    {
        previewImageSrc: 'assets/img/slideshow/4.webp',
        thumbnailImageSrc: 'assets/img/slideshow/4.webp',
        alt: 'Image of ice cream sandwich bars',
        title: 'Ice Cream Sandwich Bars'
    },
    {
        previewImageSrc: 'assets/img/slideshow/5.webp',
        thumbnailImageSrc: 'assets/img/slideshow/5.webp',
        alt: 'Image showing our beautiful outdoor table setup',
        title: 'Outdoor Table Layout - Seats up to 6 per table'
    },
    {
        previewImageSrc: 'assets/img/slideshow/6.webp',
        thumbnailImageSrc: 'assets/img/slideshow/6.webp',
        alt: 'Image of our vegan style wild dog',
        title: 'Vegan Style Wild Dog'
    },
    {
        previewImageSrc: 'assets/img/slideshow/7.webp',
        thumbnailImageSrc: 'assets/img/slideshow/7.webp',
        alt: 'Image of our bacon burger meal',
        title: 'Bacon Burger Meal'
    },
    {
        previewImageSrc: 'assets/img/slideshow/8.webp',
        thumbnailImageSrc: 'assets/img/slideshow/8.webp',
        alt: 'Image of our lasagna',
        title: 'Lasagna'
    },
];
}
