import { useQuery } from '@tanstack/react-query';
import { fetchDevices, Device } from '../api';

export default function Dashboard() {
    const { data: devices, isLoading, error } = useQuery({
        queryKey: ['devices'],
        queryFn: fetchDevices,
    });

    if (isLoading) return <div className="p-4">Loading devices...</div>;
    if (error) return <div className="p-4 text-red-500">Error loading devices</div>;

    return (
        <div className="p-8">
            <h1 className="text-2xl font-bold mb-4">Device Fleet</h1>
            <div className="overflow-x-auto border rounded-lg shadow">
                <table className="min-w-full divide-y divide-gray-200">
                    <thead className="bg-gray-50">
                        <tr>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Name</th>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Hardware ID</th>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Status</th>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">OS</th>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Last Seen</th>
                        </tr>
                    </thead>
                    <tbody className="bg-white divide-y divide-gray-200">
                        {devices?.map((device) => (
                            <tr key={device.id}>
                                <td className="px-6 py-4 whitespace-nowrap">{device.name}</td>
                                <td className="px-6 py-4 whitespace-nowrap font-mono text-sm">{device.hardwareId}</td>
                                <td className="px-6 py-4 whitespace-nowrap">
                                    <span className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${device.status === 'Online' ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'
                                        }`}>
                                        {device.status}
                                    </span>
                                </td>
                                <td className="px-6 py-4 whitespace-nowrap">{device.os}</td>
                                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                                    {new Date(device.lastSeen).toLocaleString()}
                                </td>
                            </tr>
                        ))}
                        {(!devices || devices.length === 0) && (
                            <tr>
                                <td colSpan={5} className="px-6 py-4 text-center text-gray-500">No devices found</td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>
        </div>
    );
}
