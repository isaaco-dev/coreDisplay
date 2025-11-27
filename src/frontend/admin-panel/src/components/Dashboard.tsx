import { useState, useMemo } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { fetchDevices, sendCommand, Device } from '../api';
import { Activity, Monitor, AlertTriangle, RefreshCw, Camera, Settings } from 'lucide-react';

export default function Dashboard() {
    const queryClient = useQueryClient();
    const [filterStatus, setFilterStatus] = useState('All');
    const [filterOs, setFilterOs] = useState('All');

    const { data: devices, isLoading, error } = useQuery({
        queryKey: ['devices', filterStatus, filterOs],
        queryFn: () => fetchDevices({ status: filterStatus, os: filterOs }),
        refetchInterval: 5000, // Auto-refresh every 5s
    });

    const commandMutation = useMutation({
        mutationFn: ({ id, type }: { id: string; type: string }) => sendCommand(id, type),
        onSuccess: () => {
            alert('Command sent successfully');
        },
    });

    const handleCommand = (id: string, type: string) => {
        commandMutation.mutate({ id, type });
    };

    // PERFORMANCE: Memoize stats calculation
    const stats = useMemo(() => {
        if (!devices) return { total: 0, online: 0, error: 0 };
        return {
            total: devices.length,
            online: devices.filter(d => d.status === 'Online').length,
            error: devices.filter(d => d.status === 'Error').length
        };
    }, [devices]);

    if (isLoading) return <div className="p-8 text-center text-gray-500 animate-pulse">Loading fleet data...</div>;
    if (error) return <div className="p-8 text-center text-red-500">Error loading devices. Is the backend running?</div>;

    return (
        <div className="p-8 bg-gray-50 min-h-screen">
            {/* Header & Stats */}
            <div className="mb-8">
                <h1 className="text-3xl font-bold text-gray-900 mb-4">Fleet Dashboard</h1>

                {/* Alert Banner */}
                {stats.error > 0 && (
                    <div className="bg-red-50 border-l-4 border-red-500 p-4 mb-6 rounded shadow-sm flex items-center">
                        <AlertTriangle className="text-red-500 mr-3" />
                        <div>
                            <p className="font-bold text-red-700">Attention Needed</p>
                            <p className="text-sm text-red-600">{stats.error} devices are reporting errors.</p>
                        </div>
                    </div>
                )}

                <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                    <div className="bg-white p-6 rounded-xl shadow-sm border border-gray-100">
                        <div className="flex items-center justify-between">
                            <div>
                                <p className="text-sm font-medium text-gray-500">Total Devices</p>
                                <p className="text-2xl font-bold text-gray-900">{stats.total}</p>
                            </div>
                            <Monitor className="text-blue-500 h-8 w-8" />
                        </div>
                    </div>
                    <div className="bg-white p-6 rounded-xl shadow-sm border border-gray-100">
                        <div className="flex items-center justify-between">
                            <div>
                                <p className="text-sm font-medium text-gray-500">Online</p>
                                <p className="text-2xl font-bold text-green-600">{stats.online}</p>
                            </div>
                            <Activity className="text-green-500 h-8 w-8" />
                        </div>
                    </div>
                    <div className="bg-white p-6 rounded-xl shadow-sm border border-gray-100">
                        <div className="flex items-center justify-between">
                            <div>
                                <p className="text-sm font-medium text-gray-500">Errors</p>
                                <p className="text-2xl font-bold text-red-600">{stats.error}</p>
                            </div>
                            <AlertTriangle className="text-red-500 h-8 w-8" />
                        </div>
                    </div>
                </div>
            </div>

            {/* Filters */}
            <div className="flex gap-4 mb-6">
                <select
                    value={filterStatus}
                    onChange={(e) => setFilterStatus(e.target.value)}
                    className="block w-40 pl-3 pr-10 py-2 text-base border-gray-300 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm rounded-md shadow-sm"
                >
                    <option value="All">All Status</option>
                    <option value="Online">Online</option>
                    <option value="Offline">Offline</option>
                    <option value="Error">Error</option>
                </select>
                <select
                    value={filterOs}
                    onChange={(e) => setFilterOs(e.target.value)}
                    className="block w-40 pl-3 pr-10 py-2 text-base border-gray-300 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm rounded-md shadow-sm"
                >
                    <option value="All">All OS</option>
                    <option value="Windows">Windows</option>
                    <option value="Android">Android</option>
                </select>
            </div>

            {/* Device Grid */}
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                {devices?.map((device) => (
                    <DeviceCard key={device.id} device={device} onCommand={handleCommand} />
                ))}
            </div>
        </div>
    );
}

function DeviceCard({ device, onCommand }: { device: Device; onCommand: (id: string, type: string) => void }) {
    // Mock random stats for visualization if not in API yet, or use 0
    const cpu = Math.floor(Math.random() * 100);
    const ram = Math.floor(Math.random() * 100);

    return (
        <div className="bg-white rounded-xl shadow-sm border border-gray-200 overflow-hidden hover:shadow-md transition-shadow">
            <div className="p-5">
                <div className="flex justify-between items-start mb-4">
                    <div>
                        <h3 className="font-bold text-lg text-gray-900">{device.name}</h3>
                        <p className="text-xs text-gray-500 font-mono">{device.hardwareId}</p>
                    </div>
                    <div className={`h-4 w-4 rounded-full ${device.status === 'Online' ? 'bg-green-500 shadow-[0_0_8px_rgba(34,197,94,0.6)]' :
                        device.status === 'Error' ? 'bg-red-500 animate-pulse' : 'bg-gray-400'
                        }`} />
                </div>

                <div className="space-y-3 mb-4">
                    <div>
                        <div className="flex justify-between text-xs text-gray-500 mb-1">
                            <span>CPU Usage</span>
                            <span>{cpu}%</span>
                        </div>
                        <div className="w-full bg-gray-100 rounded-full h-2">
                            <div
                                className={`h-2 rounded-full transition-all duration-500 ${cpu > 80 ? 'bg-red-500' : 'bg-blue-500'}`}
                                style={{ width: `${cpu}%` }}
                            />
                        </div>
                    </div>
                    <div>
                        <div className="flex justify-between text-xs text-gray-500 mb-1">
                            <span>RAM Usage</span>
                            <span>{ram}%</span>
                        </div>
                        <div className="w-full bg-gray-100 rounded-full h-2">
                            <div
                                className="bg-purple-500 h-2 rounded-full transition-all duration-500"
                                style={{ width: `${ram}%` }}
                            />
                        </div>
                    </div>
                </div>

                <div className="flex items-center justify-between text-xs text-gray-500 border-t pt-4">
                    <span>{device.os} • v{device.appVersion}</span>
                    <span>{new Date(device.lastSeen).toLocaleTimeString()}</span>
                </div>
            </div>

            <div className="bg-gray-50 px-5 py-3 border-t border-gray-100 flex justify-around">
                <button onClick={() => onCommand(device.id, 'Reboot')} className="p-2 text-gray-600 hover:text-red-600 hover:bg-red-50 rounded-lg transition-colors" title="Reboot">
                    <RefreshCw size={18} />
                </button>
                <button onClick={() => onCommand(device.id, 'Screenshot')} className="p-2 text-gray-600 hover:text-blue-600 hover:bg-blue-50 rounded-lg transition-colors" title="Screenshot">
                    <Camera size={18} />
                </button>
                <button onClick={() => onCommand(device.id, 'Config')} className="p-2 text-gray-600 hover:text-gray-900 hover:bg-gray-200 rounded-lg transition-colors" title="Settings">
                    <Settings size={18} />
                </button>
            </div>
        </div>
    );
}
