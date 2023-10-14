
const apiRoot = process.env.REACT_APP_API_ROOT;

export const sendHTTPGet = async (path) => {
  return await fetch(`${apiRoot}${path}`)
}

export const pullData = (response) => {
  if (response < 200 || response > 299) return null;

  return response.json();
}