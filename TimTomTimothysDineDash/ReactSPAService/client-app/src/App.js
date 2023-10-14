import { Routes, Route } from 'react-router-dom'
import './App.scss';

import NavBar from './components/navbar/navbar.component';
import HomePage from './routes/home-page/home-page.component';
import UptimeStatus from './components/uptime-status/uptime-status.component';
import MessageFeed from './components/message-feed/message-feed.component';
import { Fragment, useRef } from 'react';
import { Toast } from 'primereact/toast';

const THREE_SECONDS = 3000;

function App() {
  const toast = useRef(null);

  const createToast = (severity, summary, detail) => {
    toast.current.show({severity, summary, detail, life: THREE_SECONDS});
  }

  return (
    <Fragment>
      <NavBar />
      <Toast ref={toast} position='top-right' />
      <div className='container'>
        <UptimeStatus />
        
        <hr/>
        <div className='row gx-5'>
          <div className='col-lg-9'>
            <Routes>
              <Route path='/' element={<HomePage />} />
            </Routes>
          </div>
          
          <div className='col-lg-3'>
            <MessageFeed  />
          </div>
          
        </div>
        
      </div>
    </Fragment>
  );
}

export default App;
