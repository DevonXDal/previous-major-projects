import { Routes, Route } from 'react-router-dom'
import './App.scss';

import NavBar from './components/navbar/navbar.component';
import HomePage from './routes/home-page/home-page.component'
import { Fragment } from 'react';

function App() {
  return (
    <Fragment>
      <NavBar />
      <div className='container'>
        <Routes>
          <Route path='/' element={<HomePage />} />
        </Routes>
      </div>
    </Fragment>
  );
}

export default App;
