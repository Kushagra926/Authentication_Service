import { useEffect, useState } from "react";
import api from "./api";
import {
  PieChart, Pie, Cell, Tooltip, ResponsiveContainer
} from "recharts";

export default function Dashboard() {
  const [metrics, setMetrics] = useState(null);

  const loadMetrics = async () => {
    const res = await api.get("/api/security/metrics");
    setMetrics(res.data);
  };

  useEffect(() => {
    loadMetrics();
    const id = setInterval(loadMetrics, 5000); // live refresh
    return () => clearInterval(id);
  }, []);

  if (!metrics) return <h2>Loading...</h2>;

  return (
    <div style={{ padding: 20 }}>
      <h1>Security Operations Dashboard</h1>

      <div style={{ display: "flex", gap: 20 }}>
        <Metric title="Logins (5 min)" value={metrics.loginsLast5min} />
        <Metric title="Failed Logins" value={metrics.failedLoginsLast5min} />
        <Metric title="Blocked IPs" value={metrics.blockedIpsLast5min} />
      </div>

      <h2>OAuth Providers</h2>
      <ResponsiveContainer width="100%" height={300}>
        <PieChart>
          <Pie data={metrics.authProviders}
               dataKey="count"
               nameKey="provider"
               label>
            {metrics.authProviders.map((_, i) => (
              <Cell key={i} fill={["#0088FE", "#00C49F"][i % 2]} />
            ))}
          </Pie>
          <Tooltip />
        </PieChart>
      </ResponsiveContainer>

      <h2>Top Attackers</h2>
      <table border="1" cellPadding="10">
        <thead>
          <tr>
            <th>IP</th>
            <th>Attempts</th>
          </tr>
        </thead>
        <tbody>
          {metrics.topAttackers.map(a => (
            <tr key={a.ip}>
              <td>{a.ip}</td>
              <td>{a.attempts}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

function Metric({ title, value }) {
  return (
    <div style={{
      padding: 20,
      background: "#111",
      color: "white",
      borderRadius: 8,
      minWidth: 150
    }}>
      <h3>{title}</h3>
      <h1>{value}</h1>
    </div>
  );
}
