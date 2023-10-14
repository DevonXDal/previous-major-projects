import { Fragment, useState, useEffect } from 'react';
import runApiHealthCheck from '../../utils/api-handlers/api-health-check.util';
import './uptime-status.styles.scss';

const TEN_SECONDS = 10000;

const UptimeStatus = () => {
    const [isOnline, setIsOnline] = useState(false);

    useEffect(() => {
        const healthCheckInterval = setInterval(async () => {
            const apiStatus = await runApiHealthCheck();

            if (isOnline !== apiStatus) {
                setIsOnline(apiStatus);
            }
        }, TEN_SECONDS); 

        return () => clearInterval(healthCheckInterval); // Clear on destroy
    }, [])

    return (
    <Fragment>
        <div>
            Status: &nbsp;
            { isOnline 
                ?
                <span className="badge bg-success">Online</span>
                :
                <span className="badge bg-danger">Offline</span>
            }
            
            
        </div>
        <div className="spinner mx-3 my-2 inline"></div>
    </Fragment>
    );
}

export default UptimeStatus