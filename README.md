# NumberValidators
中国大陆证件、号码的验证类库，目前包含身份证、增值税发票、工商注册码/统一社会信用代码

[![NuGet version (NumberValidators)](https://img.shields.io/nuget/v/NumberValidators.svg?style=flat-square)](https://www.nuget.org/packages/NumberValidators/)
[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://mit-license.org/)

## .NET版本支持
目前支持以下版本：`.NET40`、`.NET Standard 2.0`

## 规范约束`IValidator<out T>`
所有验证均需实现`IValidator<out T>`接口，其中泛型`T`具备如下约束：必须继承自`ValidationResult`且需有无参构造函数，该接口定义了以下规范：
* 随机生成一个符合规则的号码 `string GenerateRandomNumber();`
* 验证号码是否正确 `T Validate(string number);`

实际验证会在此接口基础上定义额外的规范约束，且每种证件号码均默认提供`ValidatorHelper`类，用于统一验证该类型的证件号码，根据实际调用的`IValidator<out T>`实现，返回的`ValidationResult`也可能会有所不同，所有`Helper`类都提供了`AddDefaultValidator`来解决`Net Core`下反射生成实例会产生异常问题。

PS：如果验证结果`IsValid`为True，那么相应的`ValidationResult`会包含该号码可识别的所有信息，以身份证为例，返回结果会包含**行政区划（即出生登记地）、出生日期、登记序列号、校验位**

## 简单的使用示例
### 1、大陆居民身份证
默认提供`ID15Validator`（15位一代身份证）以及`ID18Validator`（18位二代身份证）两种类型的身份证验证，具体使用代码如下：
```csharp
//一代身份证验证，虽然目前未过期的一代身份证（未办理二代，如果办理了，有效期内的一代也就失效了）的仍然有效，但很多地方使用上已不被承认
valid = new ID15Validator().Validate(idNumber); 
//二代身份证验证
valid = new ID18Validator().Validate(idNumber); 
IDValidatorHelper.AddDefaultValidator(); //进行默认注册
valid = IDValidatorHelper.Validate(idNumber, ignoreCheckBit: false); //无法确认是哪种身份证时可以通过该类进行验证
```

### 2、增值税发票
默认提供`VATCode10Validator`（增值税专用发票、增值税普通发票、货物运输业增值税专用发票）以及`VATCode12Validator`（增值税普通发票[卷票]、增值税电子普通发票、区块链发票、增值税电子发票）两种长度的增值税发票验证（货物运输业增值税专用发票按国家规定目前已停用），具体使用代码如下：
```csharp
valid = new VATCode10Validator().Validate(vatCode); //增值税专用发票、增值税普通发票、货物运输业增值税专用发票验证
valid = new VATCode12Validator().Validate(vatCode); //增值税普通发票[卷票]、增值税电子普通发票验证
valid = VATCodeValidatorHelper.Validate(vatCode, minYear: 2012); //无法确认是哪种增值税发票时可以通过该类进行验证
```
注意`VATCode10Validator`返回验证结果为`VATCode10ValidationResult`，`VATCode12Validator`返回验证结果为`VATCode12ValidationResult`，`VATCodeValidatorHelper`返回验证结果为`VATCodeValidationResult`（实际也可能为`VATCode10ValidationResult`或`VATCode12ValidationResult`）

### 3、工商注册码/统一社会信用代码
默认提供`RegistrationNo15Validator`（工商注册码）以及`RegistrationNo18Validator`（法人和其他组织统一社会信用代码），具体使用代码如下：
```csharp
valid = new RegistrationNo15Validator().Validate(code); //工商注册码验证
valid = new RegistrationNo18Validator().Validate(code); //法人和其他组织统一社会信用代码验证
valid = RegistrationNoValidatorHelper.Validate(code, validLimit: null); //无法确认是工商注册码还是法人和其他组织统一社会信用代码时可以通过该类进行验证
```
注意`RegistrationNo15Validator`返回验证结果为`RegistrationNo15ValidationResult`，`RegistrationNo18Validator`返回验证结果为`RegistrationNo18ValidationResult`，`RegistrationNoValidatorHelper`返回验证结果为`RegistrationNoValidationResult`（实际也可能为`RegistrationNo15ValidationResult`或`RegistrationNo18ValidationResult`）


## Release History
**2021-04-14**
- Release v1.0.3 增加电子专票支持，Helper增加注册方法临时处理Core下反射生成实例失败问题

**2019-08-06**
- Release v1.0.2 增加区块链电子发票支持

**2019-08-05**
- Release v1.0.1 增加12位增值税普通发票支持，增加支持增值税电子发票通行费支持

**2018-05-09**
- Release v1.0.0
