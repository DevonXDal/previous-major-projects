import './quick-access-menu.styles.scss';

import { Link } from 'react-router-dom';
import { Button } from 'primereact/button';

const QuickAccessMenu = () => {
    return (
        <div className="card my-3 quick-access-menu">
            <div className="card-body">
                <h5 className="card-title">Quick Access Menu</h5>
                <h6 className="card-subtitle">Press any Button to jump to the related page</h6>
                <div className="row mt-2">
                    <div className="col-lg-3 col-6">
                        Host Staff:
                        <hr/>
                        <Link to='/Host'><Button  type="Button" label="Main Page" severity='help' className="my-1 w-100"></Button></Link>
                        <Link to='/Host/AddToWaitQueue'><Button  type="Button" label="Add to Wait Queue" severity='help' className=" my-1 w-100"></Button></Link>
                        <Link to='/Host/SeatATable'><Button  type="Button" label="Seat a Table" severity='help' className=" my-1 w-100"></Button></Link>
                    </div>
                    <div className="col-lg-3 col-6">
                        Wait Staff:
                        <hr/>
                        <Link to='/Wait'><Button  type="Button" label="Main Page" severity='help' className=" my-1 w-100"></Button></Link>
                        <Link to='/Wait/InsertTakenOrder'><Button  type="Button" severity='help' label="Take Table's Order" className=" my-1 w-100"></Button></Link>
                        <Link to='/Wait/MarkOrderPaid'><Button  type="Button" severity='help' label="Mark Table as Paid" className=" my-1 w-100"></Button></Link>
                    </div>
                    <div className="col-lg-3 col-6">
                        Kitchen Staff:
                        <hr/>
                        <Link to='/Kitchen'><Button  type="Button" label="Main Page" severity='help' className=" my-1 w-100"></Button></Link>
                        <Link to='/Kitchen/StartCookingOrder'><Button  type="Button" severity='help' label="Start on Order" className=" my-1 w-100"></Button></Link>
                        <Link to='/Kitchen/MarkOrderFinished'><Button  type="Button" severity='help' label="Mark Order as Finished" className=" my-1 w-100"></Button></Link>
                    </div>
                    <div className="col-lg-3 col-6">
                        Bus Staff:
                        <hr/>
                        <Link to='/Bus'><Button  type="Button" label="Main Page" severity='help' className=" my-1 w-100"></Button></Link>
                        <Link to='/Bus/MarkTableAsCleaned'><Button  type="Button" severity='help' label="Mark Table as Cleaned" className=" my-1 w-100"></Button></Link>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default QuickAccessMenu;