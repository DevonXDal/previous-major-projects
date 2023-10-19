
const apiRoot = process.env.REACT_APP_API_ROOT;
const runFetch = async (path, config = null) => {

    if (config) {
        return await fetch(`${apiRoot}${path}`, config)
          .then(response => {
            console.log(response);
            return response;
          })
          .catch((e) => 
          {
            console.error(e);
            return null;
          }); 
    }

    return await fetch(`${apiRoot}${path}`)
      .then(response => {
        console.log(response);
        return response;
      })
      .catch((e) => 
      {
        console.error(e);
        return null;
      });
}

/**
 * Sends a HTTP GET request to the API and returns the response.
 * @param {string} path The path from the API root including the initial slash, '/...'
 * @returns Returns the response if able, otherwise it will return null
 */
export const sendHTTPGet = async (path) => {
  return await runFetch(path);
}

/**
 * Sends a HTTP POST request to the API and returns the response.
 * @param {string} path The path from the API root including the initial slash, '/...'
 * @param {string | FormData} data The data as a JSON string or form data to send in the request
 * @returns Returns the response if able, otherwise it will return null
 */
export const sendHTTPPost = async (path, data) => {
    const requestOptions = {
      method: 'POST',
      body: data
    }; 
    return await runFetch(path, requestOptions);
}

/**
 * Sends a HTTP PUT request to the API and returns the response.
 * @param {string} path The path from the API root including the initial slash, '/...'
 * @param {string | FormData} data The data as JSON string or form data to send in the request
 * @returns Returns the response if able, otherwise it will return null
 */
export const sendHTTPPut = async (path, data) => {
    const requestOptions = {
      method: 'PUT',
      //headers: {'Content-Type': 'multipart/form-data'},
      body: data
    };
    console.log(data);

    return await runFetch(path, requestOptions);
}

/**
 * Returns the data from a HTTP response if able.
 * @param {Response | null} response The response from the API request.
 * @returns Either the data from the response as JSON or null if no data can be pulled
 */
export const pullData = async (pendingResponse) => {
  let response = await pendingResponse;
  if (!response || response.status < 200 || response.status > 299) return null;

  return response.json() ?? [];
}

