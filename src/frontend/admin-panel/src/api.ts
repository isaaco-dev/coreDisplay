import axios from 'axios';

const api = axios.create({
    baseURL: 'http://localhost:5000/api/v1',
    headers: {
        'Content-Type': 'application/json',
    },
});

export interface Device {
    id: string;
    hardwareId: string;
    name: string;
    status: 'Online' | 'Offline' | 'Error' | 'Maintenance';
    os: string;
    appVersion: string;
    lastSeen: string;
    configuration: string;
}

export const fetchDevices = async (): Promise<Device[]> => {
    // Mocking response for now as we don't have a list endpoint yet, 
    // or assuming one will be created.
    // In a real scenario, we would call: const response = await api.get('/devices');
    // return response.data;

    // Returning empty array or mock data if endpoint missing
    return [];
};

export default api;
