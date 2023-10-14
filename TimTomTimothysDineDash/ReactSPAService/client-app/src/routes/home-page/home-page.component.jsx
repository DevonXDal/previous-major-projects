import './home-page.styles.scss';
import { Galleria } from 'primereact/galleria';
import QuickAccessMenu from '../../components/quick-access-menu/quick-access-menu.component';

// TODO: Try to import dynamically
import img1 from '../../assets/img/slideshow/1.webp';
import img2 from '../../assets/img/slideshow/2.webp';
import img3 from '../../assets/img/slideshow/3.webp';
import img4 from '../../assets/img/slideshow/4.webp';
import img5 from '../../assets/img/slideshow/5.webp';
import img6 from '../../assets/img/slideshow/6.webp';
import img7 from '../../assets/img/slideshow/7.webp';
import img8 from '../../assets/img/slideshow/8.webp';

const images = [
    {
        previewImageSrc: img1,
        thumbnailImageSrc: img1,
        alt: 'Logo: Finest quality 1-star dining',
        title: 'Dine&Dash Logo'
    },
    {
        previewImageSrc: img2,
        thumbnailImageSrc: img2,
        alt: 'Image of a bowl of ice cream',
        title: 'Ice Cream Bowl'
    },
    {
        previewImageSrc: img3,
        thumbnailImageSrc: img3,
        alt: 'Image of the boneless fried chicken meal',
        title: 'Boneless Fried Chicken Meal'
    },
    {
        previewImageSrc: img4,
        thumbnailImageSrc: img4,
        alt: 'Image of ice cream sandwich bars',
        title: 'Ice Cream Sandwich Bars'
    },
    {
        previewImageSrc: img5,
        thumbnailImageSrc: img5,
        alt: 'Image showing our beautiful outdoor table setup',
        title: 'Outdoor Table Layout - Seats up to 6 per table'
    },
    {
        previewImageSrc: img6,
        thumbnailImageSrc: img6,
        alt: 'Image of our vegan style wild dog',
        title: 'Vegan Style Wild Dog'
    },
    {
        previewImageSrc: img7,
        thumbnailImageSrc: img7,
        alt: 'Image of our bacon burger meal',
        title: 'Bacon Burger Meal'
    },
    {
        previewImageSrc: img8,
        thumbnailImageSrc: img8,
        alt: 'Image of our lasagna',
        title: 'Lasagna'
    },
];

const itemTemplate = (item) => {
  return <img key={item.title + ' main'} src={item.previewImageSrc} alt={item.alt} title={item.title}/>
}

const thumbnailTemplate = (item) => {
  return (
    <div className="grid grid-nogutter justify-content-center galleria-thumbnail">
        <img className="galleria-thumbnail" key={item.title + ' thumbnail'} src={item.thumbnailImageSrc} alt={item.alt} title={item.title} />
    </div>
  );
}

const HomePage = () => {
  return (
    <div>
        <h2>Front Page <i className={'bi bi-star'}></i></h2>

        <Galleria 
        value={images}  
        item={itemTemplate} 
        thumbnail={thumbnailTemplate} 
        circular={true} 
        autoPlay={true} 
        transitionInterval={5000}
        />
        <QuickAccessMenu />
    </div>
  );
}

export default HomePage;