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

export interface DeviceFilters {
    status?: string;
    os?: string;
}

export const fetchDevices = async (filters?: DeviceFilters): Promise<Device[]> => {
    const params = new URLSearchParams();
    if (filters?.status && filters.status !== 'All') params.append('status', filters.status);
    if (filters?.os && filters.os !== 'All') params.append('os', filters.os);

    const response = await api.get('/devices', { params });
    return response.data;
};

export const sendCommand = async (deviceId: string, type: string, payload?: string) => {
    return api.post(`/devices/${deviceId}/command`, { type, payload });
};

export default api;
