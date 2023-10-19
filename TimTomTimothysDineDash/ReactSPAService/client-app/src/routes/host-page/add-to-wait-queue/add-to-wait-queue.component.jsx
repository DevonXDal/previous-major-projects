import { useState, Fragment } from "react";
import { ProgressSpinner } from 'primereact/progressspinner';
import { Slider } from 'primereact/slider';
import { insertPartyIntoQueue } from "../../../utils/api-handlers/api-host-page-handlers.util";

const AddToWaitQueue = ({toastHandler}) => {
    const [isLoading, setIsLoading] = useState(false);
    const [numOfCustomers, setNumOfCustomers] = useState(1); // One customer by default

    const handleSubmit = async (e) => {
        e.preventDefault();
        setIsLoading(true);

        if (numOfCustomers < 1 || numOfCustomers > 6) return;

        let result = await insertPartyIntoQueue(numOfCustomers);
        
        if (result) { // It failed
            toastHandler('error', 'Failed', result);
        } else { // No message in result is indicative of success
            toastHandler('success', 'Success', 'The customer group has been added to the wait queue');
            setNumOfCustomers(1);
        }
        
        
        setIsLoading(false);
    }

    const buildNumSelectedIndicator = () => {
      const numSelectedSlots = [];

      for (let j = 1; j <= numOfCustomers; j++) {
        numSelectedSlots.push(<div className="col-2 py-3" key={`Item ${j}`}><i className="bi bi-person-fill fs-4"></i> {j}</div>);
      }

      return numSelectedSlots;
    }

  return (
    <form onSubmit={handleSubmit}>
        <div className="card">
            <div className="card-header text-center fw-bold">Add a Group to the Wait Queue</div>
            <div className="card-body">
                <label htmlFor="numOfCustomers" className="my-2">How many customers are in this group?</label>
                <Slider value={numOfCustomers} onChange={(e) => setNumOfCustomers(e.value)} step={1} min={1} max={6} />
                <div className="row">
                    {buildNumSelectedIndicator()}
                </div>
            </div>
            <div className="card-footer row">
                <div className="col-4"></div>
                <div className="col-4 text-center">
                    {isLoading 
                        ?
                        <ProgressSpinner />
                        :
                        <button className="btn btn-primary m-100" type="submit">Add Party</button>
                    }
                </div>
                <div className="col-4"></div>
            </div>
        </div>
    </form>
  );
}

export default AddToWaitQueue;