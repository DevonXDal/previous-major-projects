import './navbar.styles.scss'
import logo from '../../assets/img/logo/tim-tom-timothys-dinedash-logo.webp';
import {useState} from 'react';
import { Link, NavLink } from 'react-router-dom';

/**
 * Renders the navigation bar that appears along the top of the screen showing the logo,
 * title and main page paths.
 */
const NavBar = () => {
  const [isExpanded, setIsExpanded] = useState(false);
  
  const toggle = () => {
    setIsExpanded(!isExpanded);
  }
  const collapse = () => {
    setIsExpanded(false);
  }

  return (
    <header>
        <nav
            className="navbar navbar-expand-sm navbar-toggleable-sm border-bottom box-shadow mb-3"
        >
            <div className="container">
            <Link className="navbar-brand" to='/'>
                <img className="rounded" width="50" style={{marginRight: '0.33rem'}} src={logo} alt="Logo: Tim Tom Timothy's Dine&Dash" />
                Tim Tom Timothy's Dine&Dash 
            </Link>
            <button
                className="navbar-toggler"
                type="button"
                data-toggle={collapse}
                data-target=".navbar-collapse"
                aria-label="Toggle navigation"
                aria-expanded={isExpanded}
                onClick={toggle}
            >
                <span className="navbar-toggler-icon"></span>
            </button>
            <div
                className={`navbar-collapse collapse d-sm-inline-flex justify-content-end ${
                    isExpanded && 'show'
                }`}
            >
                <ul className="navbar-nav flex-grow">
                <li className="nav-item">
                    <NavLink exact className={({isActive}) => (isActive ? 'nav-link nav-link-active' : 'nav-link') } to='/'>Home</NavLink>
                </li>
                <li className="nav-item" >
                    <NavLink  className={({isActive}) => (isActive ? 'nav-link nav-link-active' : 'nav-link') } to='/Host'>Host</NavLink>
                    
                </li>
                <li className="nav-item" active="'nav-link-active'">
                    <NavLink className={({isActive}) => (isActive ? 'nav-link nav-link-active' : 'nav-link') } to='/Wait'>Wait</NavLink>
                    
                </li>
                <li className="nav-item" >
                    <NavLink className="nav-link " to='/Kitchen'>Kitchen</NavLink>
                    
                </li>
                <li className="nav-item" >
                    <NavLink className="nav-link " to='/Bus'>Bus</NavLink>

                </li>
                </ul>
            </div>
            </div>
        </nav>
        </header>
  );
}

export default NavBar;