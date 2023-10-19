import { sendHTTPGet, sendHTTPPut, pullData } from "./api-http-handlers.util"

export const fetchHostStaffStatus = async () => {
    return await pullData(sendHTTPGet(`/${process.env.REACT_APP_API_VERSION}/HostStaff/Status`));
}

export const insertPartyIntoQueue = async (numOfCustomers) => {
    let formData = new FormData(); // Otherwise data is sent as just JSON
    formData.append('numOfCustomers', numOfCustomers.toString()); 
    

    console.log(formData);
    const response = await 
        sendHTTPPut(
            `/${process.env.REACT_APP_API_VERSION}/HostStaff/DeclareNewCustomersInQueue`,
            formData
        );

    if (!response) return 'No response from the API';
    if (response.status !== 200) return response.statusText;

    return null;
}

export const seatNextPartyAtTable = async (tableId) => {
    const data = { tableId };

    const response = await 
        sendHTTPPut(
            `/${process.env.REACT_APP_API_VERSION}/HostStaff/SeatNextGroupAtTable`,
            JSON.stringify(data)
        );

    if (!response) return 'No response from the API';
    if (response.status !== 200) return response.statusText;

    return null;
}