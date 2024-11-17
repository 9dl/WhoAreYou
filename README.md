## WhoAreYou - Multi-Functional Vulnerability Testing Tool

![image](https://github.com/user-attachments/assets/5577cc07-de78-4e4c-87f4-0b5cd0a42938)

### Overview

**WhoAreYou** is a powerful multi-functional vulnerability testing tool developed in NET Core 9 designed to assist
security researchers and penetration testers in identifying common web application vulnerabilities.

### Features

#### **1. LFI Vulnerability Scanner**

The LFI scanner helps to identify **Local File Inclusion** vulnerabilities in web applications. It works by appending a
series of payloads to a given URL and checking if sensitive files, such as `/etc/passwd`, can be accessed. This feature
includes:

- **Payload Generation**: The tool can generate LFI payloads with multiple bypass techniques:
    - **Null byte bypass**
    - **Current directory bypass**
    - **Double dot (..) bypass**
    - **Directory traversal** (with configurable max depth)
    - **Unicode bypass** (only for Linux systems)
- **Testing URLs**: The scanner tests the URLs and checks for the presence of sensitive files to indicate potential LFI
  vulnerabilities.

#### **2. Web Scraper**

The Web Scraper module is used to extract all links from a specified webpage. It is useful for identifying additional
targets for testing or simply gathering information about a web application. Features include:

- **Scraping Links**: Extracts all the links (internal and external) found on the page.
- **Host Matching**: Optionally filters links to only include those from the same domain as the input URL.
- **Max Sites Limit**: Allows you to set a limit on the number of links to scrape (with an option to scrape all links if
  set to `0`).
- **HTML Fetching**: Efficiently fetches the HTML content of the target URL for link extraction.

### Future Features

**WhoAreYou** will continue to evolve and expand with new features, including:

- **RFI**: For identifying RFI vulnerabilities. (duhh)
- **SSRF**: For identifying RFI vulnerabilities. (duhh)
- **XSS**: For identifying XSS vulnerabilities. (duhh)
