import { defineBoot } from '#q-app/wrappers';
import axios from 'axios';
const api = axios.create({ baseURL: 'http://localhost:9876/api' }); // This should be pulled from an environment file in a more realistic example

export default defineBoot((/*{ app }*/) => {
  // In a more detailed example with authentication etc we could have interceptors here catching 401 responses and redirecting to a login page.
});

export { api };
