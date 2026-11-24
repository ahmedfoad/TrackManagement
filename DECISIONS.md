# DECISIONS

1. **What did AI generate for you, and what did you write or modify yourself?**  
   AI generated the authentication service and seed methods for creating dummy data and a single test user. It also helped with frontend styling, the `AuthGuard`, and testing. I validated the database context and API logic before running migrations.

2. **What security issues did you find (or introduce) in the AI-generated code? How did you handle them?**  
   I found that the APIs needed authorization, which AI had not accounted for. I added the authorization requirements manually. AI also omitted CORS configuration, which I added. I found no other security issues and introduced no other security changes.

3. **One thing the AI got wrong is that you had to fix. What was it and why was it wrong?**  
   AI did not account for the authorization required by the APIs, so I added it manually. It also omitted CORS configuration, which I fixed. Additionally, AI did not reflect the data-model decision to save a county as text in a column rather than split it into a separate table.
