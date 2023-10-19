import { Fragment, useState, useEffect } from 'react';
import runApiHealthCheck from '../../utils/api-handlers/api-health-check.util';
import './uptime-status.styles.scss';

const TEN_SECONDS = 10000;

const UptimeStatus = ({toastHandler}) => {
    const [isOnline, setIsOnline] = useState(false);

    const handleOnlineStatus = (apiStatus, showToasts) => {
        if (isOnline !== apiStatus) {
            if (showToasts) {
                if (apiStatus) {
                    toastHandler('info', 'Connection Reestablished', 'The system can now communicate with the API');
                } else {
                    toastHandler('warn', 'Connection Lost', 'The system is currently unable to communicate with the API');
                }
            }

            setIsOnline(apiStatus);
        }
    }

    // Lesson Learned: never use a run once useEffect with an interval
    // it will only see the old variable due to stale closure.
    // useEffect will clear the interval to restart it when the variable is set
    useEffect(() => {
        const healthCheckInterval = setInterval(async () => {
            performHealthCheck(true);
        }, TEN_SECONDS); 

        const performHealthCheck = async (showToasts) => {
            const apiStatus = await runApiHealthCheck();
            handleOnlineStatus(apiStatus, showToasts);
        }
        
        performHealthCheck(false);
        return () => clearInterval(healthCheckInterval); // Clear on destroy
    }, [isOnline]);

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