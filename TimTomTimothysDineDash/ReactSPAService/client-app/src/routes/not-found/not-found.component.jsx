import { Link } from "react-router-dom";

const NotFound = () => {
  return (
    <div>
        <h1>Sorry, that page was not found</h1>
        <Link to='/'>Return Home?</Link>
    </div>
  );
}

export default NotFound;