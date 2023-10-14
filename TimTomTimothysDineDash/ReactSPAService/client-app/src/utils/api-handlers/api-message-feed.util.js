import { sendHTTPGet, pullData } from "./api-http-handlers.util"

export const fetchApiMessageFeed = async () => {
    return await pullData(sendHTTPGet(`/${process.env.REACT_APP_API_VERSION}/MessageFeed/Status`));
}