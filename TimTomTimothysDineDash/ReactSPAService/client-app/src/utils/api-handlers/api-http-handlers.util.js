
const apiRoot = process.env.REACT_APP_API_ROOT;
const runFetch = async (path, config = null) => {

    if (config) {
        return await fetch(`${apiRoot}${path}`, config)
          .then(response => {
            return response;
          })
          .catch(() => 
          {
            return null;
          }); 
    }

    return await fetch(`${apiRoot}${path}`)
      .then(response => {
        return response;
      })
      .catch((e) => 
      {
        console.log(e);
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
 * @param {json} data The data as JSON to send in the request
 * @returns Returns the response if able, otherwise it will return null
 */
export const sendHTTPPost = async (path, data) => {
    return await fetch(`${apiRoot}${path}`, {method: 'POST', body: data});
}

/**
 * Sends a HTTP PUT request to the API and returns the response.
 * @param {string} path The path from the API root including the initial slash, '/...'
 * @param {json} data The data as JSON to send in the request
 * @returns Returns the response if able, otherwise it will return null
 */
export const sendHTTPPut = async (path, data) => {
    return await fetch(`${apiRoot}${path}`, {method: 'PUT', body: data});
}

/**
 * Returns the data from a HTTP response if able.
 * @param {Response | null} response The response from the API request.
 * @returns Either the data from the response as JSON or null if no data can be pulled
 */
export const pullData = async (pendingResponse) => {
  let response = await pendingResponse;
  console.log(response);
  if (!response || response.status < 200 || response.status > 299) return null;

  return response.json() ?? [];
}

