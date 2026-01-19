// // import React, { useState } from "react";
// // import axios from "axios";
// // import { useNavigate } from "react-router-dom";

// // const Login = () => {
// //   const [email, setEmail] = useState("");
// //   const [password, setPassword] = useState("");
// //   const [error, setError] = useState("");
// //   const navigate = useNavigate();

// //   const handleLogin = async (e) => {
// //     e.preventDefault();
// //     setError("");

// //     try {
// //       const response = await axios.post(
// //         "https://localhost:7240/api/authentication/login-user",
// //         {
// //           email,
// //           password,
// //         }
// //       );

      
// //       localStorage.setItem("token", response.data.token);
// //       localStorage.setItem("refreshToken", response.data.refreshToken);

      
// //       navigate("/dashboard");

// //     } catch (err) {
// //       setError("Invalid email or password");
// //     }
// //   };

// //   const handleGoogleLogin = () => {
// //     window.location.href =
// //       "https://localhost:7240/api/authentication/google/login";
// //   };

// //   return (
// //     <div style={{ width: "350px", margin: "100px auto" }}>
// //       <h2>Login</h2>

// //       {error && <p style={{ color: "red" }}>{error}</p>}

// //       <form onSubmit={handleLogin}>
// //         <div>
// //           <label>Email</label>
// //           <input
// //             type="email"
// //             value={email}
// //             onChange={(e) => setEmail(e.target.value)}
// //             required
// //           />
// //         </div>

// //         <div style={{ marginTop: "10px" }}>
// //           <label>Password</label>
// //           <input
// //             type="password"
// //             value={password}
// //             onChange={(e) => setPassword(e.target.value)}
// //             required
// //           />
// //         </div>

// //         <button type="submit" style={{ marginTop: "15px" }}>
// //           Login
// //         </button>
// //       </form>

// //       <hr style={{ margin: "20px 0" }} />

// //       <button onClick={handleGoogleLogin}>
// //         Login with Google
// //       </button>
// //     </div>
// //   );
// // };

// // export default Login;


// import { useState } from "react";
// import api from "./api";
// import { useNavigate } from "react-router-dom";

// export default function Login() {
//   const [email, setEmail] = useState("");
//   const [password, setPassword] = useState("");
//   const navigate = useNavigate();

//   const handleLogin = async (e) => {
//     e.preventDefault();

//     try {
//       const res = await api.post("/api/authentication/login-user", {
//         email,
//         password,
//       });

//       // 🔥 THIS IS THE CRITICAL LINE YOU WERE MISSING
//       localStorage.setItem("token", res.data.token);

//       // optional: refresh token
//       localStorage.setItem("refreshToken", res.data.refreshToken);

//       navigate("/dashboard");
//     } catch (err) {
//       alert("Login failed");
//       console.error(err);
//     }
//   };

//   return (
//     <form onSubmit={handleLogin}>
//       <h2>Login</h2>

//       <input
//         placeholder="Email"
//         value={email}
//         onChange={(e) => setEmail(e.target.value)}
//       />

//       <input
//         type="password"
//         placeholder="Password"
//         value={password}
//         onChange={(e) => setPassword(e.target.value)}
//       />

//       <button type="submit">Login</button>
//     </form>
//   );
// }


import { useState } from "react";
import api from "./api";
import { useNavigate } from "react-router-dom";

export default function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();

    try {
      const res = await api.post("/api/authentication/login-user", {
        email,
        password,
      });

      // 🔥 THIS WAS MISSING
      localStorage.setItem("token", res.data.token);

      // go to dashboard
      navigate("/dashboard");
    } catch (err) {
      setError("Invalid login");
    }
  };

  return (
    <div style={{ maxWidth: 400, margin: "100px auto" }}>
      <h2>Login</h2>

      {error && <p style={{ color: "red" }}>{error}</p>}

      <form onSubmit={handleLogin}>
        <input
          placeholder="Email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          style={{ width: "100%", padding: 10, marginBottom: 10 }}
        />

        <input
          placeholder="Password"
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          style={{ width: "100%", padding: 10, marginBottom: 10 }}
        />

        <button style={{ width: "100%", padding: 10 }}>
          Login
        </button>
      </form>
    </div>
  );
}
