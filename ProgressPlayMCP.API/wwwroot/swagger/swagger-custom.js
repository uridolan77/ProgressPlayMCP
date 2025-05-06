// Auto-set bearer token from login response
window.onload = function() {
    // Hook into Swagger UI's response handling
    const originalSwaggerUiStandalonePreset = window.ui.getPlugins()["Standalone"];
    if (originalSwaggerUiStandalonePreset) {
        const originalResponseInterceptor = originalSwaggerUiStandalonePreset.responseInterceptor;
        originalSwaggerUiStandalonePreset.responseInterceptor = function(response) {
            // Call the original interceptor
            const originalResponse = originalResponseInterceptor ? originalResponseInterceptor(response) : response;
            
            // Check if this is a response from the login endpoint
            const url = response.url;
            if (url && url.endsWith('/api/Auth/login')) {
                try {
                    // Parse the response JSON
                    const jsonResponse = JSON.parse(response.text);
                    
                    // Check if we got a token
                    if (jsonResponse && jsonResponse.token) {
                        const token = jsonResponse.token;
                        console.log("Automatically setting bearer token from login response");
                        
                        // Set the token in Swagger UI's auth
                        const authDefinitions = ui.getSystem().authSelectors.definitionsToAuthorize();
                        if (authDefinitions) {
                            const bearerAuth = authDefinitions.find(auth => auth.get("name") === "Bearer");
                            if (bearerAuth) {
                                // Prepare the auth data with the token
                                const authData = {
                                    "Bearer": {
                                        name: "Bearer",
                                        schema: bearerAuth.get("schema").toJS(),
                                        value: `Bearer ${token}`
                                    }
                                };
                                
                                // Apply the auth
                                ui.getSystem().authActions.authorize(authData);
                                console.log("Bearer token set automatically");
                            }
                        }
                    }
                } catch (e) {
                    console.error("Error automatically setting bearer token", e);
                }
            }
            
            return originalResponse;
        };
    }
};