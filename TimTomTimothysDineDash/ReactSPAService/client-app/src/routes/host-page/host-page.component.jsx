import { Fragment, useState, useEffect } from "react"
import { Link, Outlet } from 'react-router-dom';
import { fetchHostStaffStatus } from '../../utils/api-handlers/api-host-page-handlers.util';

const TEN_SECONDS = 10000;

const HostPage = ({toastHandler}) => {
    const [showData, setShowData] = useState(true);
    const [status, setStatus] = useState(null);

    useEffect(() => {
        const hostStaffUpdatesInterval = setInterval(async () => {
            doStatusCheck();
        }, TEN_SECONDS); 

        const doStatusCheck = async () => {
            const potentialStatus = await fetchHostStaffStatus();

            if (!potentialStatus) return;

            if (potentialStatus && status !== potentialStatus) {
                setStatus(potentialStatus);
            }
        }
        
        doStatusCheck();

        return () => clearInterval(hostStaffUpdatesInterval); // Clear on destroy
    }, [status]);

    const toggleData = () => {
      setShowData(!showData);
    }




  return (
    <Fragment>
    <h1>Host Staff <i className="bi bi-door-open"></i></h1>
    <h5>Checks every 15 seconds</h5>

    {/* Button row */}
<div className="row mt-2 mb-4">
    <div className="col-4">
        <Link to="/Host/AddToWaitQueue" className="btn btn-info w-75" role="button">Add to Wait Queue</Link>
    </div>
    <div className="col-4">
        <button className="btn btn-info w-75" type="button" onClick={toggleData}>{showData ? 'Hide Data' : 'Show Data'}</button>
    </div>
    <div className="col-4">
        <Link to="/Host/SeatATable" className="btn btn-info w-75" role="button">Seat a Table</Link>
    </div>
</div>

<div className="row gx-5 mb-4" hidden={!showData}>

{/* Tables of parties in the queue */}
<div className="card col-12 col-lg-6">
    <div className="card-header">
        Parties in the Queue
    </div>
    <div className="card-body table-responsive">
        {status && (
            <table className="table mb-0">
            <thead>
                <tr>
                    <th>
                        Party Id
                    </th>
                    <th>
                        Number of People
                    </th>
                    <th>
                        Time of Arrival
                    </th>
                </tr>
            </thead>
            <tbody>
            {
                status.QueueParties.map(party => {
                    return (
                    <tr key={party.id}>
                        <td>
                            {party.id}
                        </td>
                        <td>
                            {party.numToSeat}
                        </td>
                        <td>
                            {party.lastUpdate}
                        </td>
                    </tr>
                    );
                })
            }
            </tbody>
        </table>
        )}
        {!status && 
            <b>Waiting for status from server</b>
        }
        
    </div>
</div>

{/* Cards indicating table status */}
{status &&
    <div className="col-lg-6 col-12">
    <div className="row">
        {
            status.Tables.map(table => {
                return (
                    <div key={table.id} className="card mb-2" >
                        <div className="card-header">
                            Table {table.id}
                        </div>
                        <div className="card-body table-responsive">
                            Is Available: {(table.isAvailable) ? 'Yes' : 'No'}
                        </div>
                        <div className="card-footer">Last Update: {table.lastUpdate}</div>
                    </div>
                );
            })
        }
    </div>
    </div>
}

        

</div>
<Outlet />

</Fragment>
);
}

export default HostPage;