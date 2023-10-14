import { sendHTTPGet } from "./api-http-handlers.util";

const runApiHealthCheck = async () => {
    const result = await sendHTTPGet('/Is-Alive');

    return !!result; //is result not null
}

export default runApiHealthCheck;